using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using The_Movies_WPF_app.Repositories;
using The_Movies_WPF_app.ViewModels;
using The_Movies_WPF_app.Views;

namespace The_Movies_WPF_app
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // --- Repositories (Singletons) ---
            // Vi registrerer fil-repos som singleton, da de peger på den samme fil.
            services.AddSingleton<IMovieRepository>(new FileMovieRepository("movies.csv"));
            services.AddSingleton<ICinemaRepository>(new FileCinemaRepository("cinemas.csv"));
            services.AddSingleton<IAuditoriumRepository>(new FileAuditoriumRepository("auditoriums.csv"));

            // FileScreeningRepository afhænger selv af IMovieRepository. 
            // Containeren finder selv ud af at "injicere" den i constructoren.
            services.AddSingleton<IScreeningRepository, FileScreeningRepository>();

            // --- ViewModels (Transient) ---
            // Vi vil have en ny ViewModel, hver gang et vindue åbnes.
            services.AddTransient<RegisterMovieViewModel>();
            services.AddTransient<RegisterScreeningViewModel>();
            services.AddTransient<MonthlyScheduleViewModel>();

            // --- Views (Transient) ---
            // Vi vil have et nyt vindue, hver gang det kaldes.
            services.AddTransient<RegisterMovieView>();
            services.AddTransient<RegisterScreeningView>();
            services.AddTransient<MonthlyScheduleView>();
            services.AddTransient<FrontPage>(); // Vigtigt: Hovedsiden skal også med.
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Bed containeren om at starte hovedsiden.
            var frontPage = ServiceProvider.GetService<FrontPage>();
            frontPage?.Show();
        }
    }
}