using System.Windows;
using The_Movies_WPF_app.Repositories;
using The_Movies_WPF_app.ViewModels;

namespace The_Movies_WPF_app.Views
{
    public partial class MonthlyScheduleView : Window
    {
        public MonthlyScheduleView()
        {
            InitializeComponent();

            // Opret repositories
            var movieRepo = new FileMovieRepository();
            var cinemaRepo = new FileCinemaRepository();
            var auditoriumRepo = new FileAuditoriumRepository();
            var screeningRepo = new FileScreeningRepository(movieRepo); // Screening repo afhænger af movie repo

            // Sæt DataContext til ViewModel
            DataContext = new MonthlyScheduleViewModel(cinemaRepo, screeningRepo, movieRepo, auditoriumRepo);
        }

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}