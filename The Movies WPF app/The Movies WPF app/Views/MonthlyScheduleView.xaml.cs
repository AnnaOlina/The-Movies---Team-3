using System.Windows;
using The_Movies_WPF_app.Repositories;
using The_Movies_WPF_app.ViewModels;

namespace The_Movies_WPF_app.Views
{
    public partial class MonthlyScheduleView : Window
    {
        public MonthlyScheduleView(MonthlyScheduleViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}