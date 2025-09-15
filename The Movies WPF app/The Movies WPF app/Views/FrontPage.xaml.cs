using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace The_Movies_WPF_app.Views
{
    public partial class FrontPage : Window
    {
        public FrontPage()
        {
            InitializeComponent();
        }

        private void Movie_Button_Click(object sender, RoutedEventArgs e)
        {
            // Bed containeren om at bygge vinduet.
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