using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace The_Movies_WPF_app.Views
{
    /// <summary>
    /// Interaction logic for FrontPage.xaml
    /// </summary>
    public partial class FrontPage : Window
    {
        public FrontPage()
        {
            InitializeComponent();
        }

        private void Movie_Button_Click(object sender, RoutedEventArgs e)
        {
            // Bed containeren om at bygge vinduet. 
            // Containeren vil automatisk:
            // 1. Oprette RegisterMovieView
            // 2. Oprette RegisterMovieViewModel
            // 3. Finde det registrerede IMovieRepository (FileMovieRepository)
            // 4. Give (3) til (2), og sætte (2) som DataContext for (1) (hvis du opsætter View'et til det)

            var movieWindow = App.ServiceProvider.GetService<RegisterMovieView>();
            movieWindow?.Show();
        }

        private void MonthlySchedule_Button_Click(object sender, RoutedEventArgs e)
        {
            var mScheduleWindow = App.ServiceProvider.GetService<MonthlyScheduleView>();
            mScheduleWindow?.Show();
        }

        private void Screening_Button_Click(object sender, RoutedEventArgs e)
        {
            var screeningWindow = App.ServiceProvider.GetService<RegisterScreeningView>();
            screeningWindow?.Show();
        }
    }
}
