using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using The_Movies_WPF_app.Repositories;
using The_Movies_WPF_app.ViewModels;
using The_Movies_WPF_app.Views;

namespace The_Movies_WPF_app
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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
            // 1. Registrer Repositories (som Singletons - de skal kun leve én gang)
            // Når nogen beder om IMovieRepository, skal de have FileMovieRepository.
            services.AddSingleton<IMovieRepository>(new FileMovieRepository("movies.csv"));
            services.AddSingleton<ICinemaRepository>(new FileCinemaRepository("cinemas.csv"));
            services.AddSingleton<IAuditoriumRepository>(new FileAuditoriumRepository("auditoriums.csv"));

            // ScreeningRepo afhænger selv af IMovieRepository. Containeren løser selv dette.
            services.AddSingleton<IScreeningRepository, FileScreeningRepository>();

            // 2. Registrer ViewModels (som Transient - én ny hver gang vi beder om én)
            services.AddTransient<RegisterMovieViewModel>();
            services.AddTransient<RegisterScreeningViewModel>();
            services.AddTransient<MonthlyScheduleViewModel>();

            // 3. Registrer Views (Vinduer)
            services.AddTransient<RegisterMovieView>();
            services.AddTransient<RegisterScreeningView>();
            services.AddTransient<MonthlyScheduleView>();
            services.AddTransient<FrontPage>(); // Din hovedside
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Start applikationen ved at bede containeren om hovedsiden
            var frontPage = ServiceProvider.GetService<FrontPage>();
            frontPage?.Show();
        }
    }
}
