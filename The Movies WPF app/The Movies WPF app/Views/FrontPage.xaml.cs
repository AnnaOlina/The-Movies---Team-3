using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            RegisterMovieView movieWindow = new RegisterMovieView();
            movieWindow.Show();
        }
        //private void MonthlyPlan_Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    MonthlyPLanView mPlanWindow = new MonthlyPlanView();
        //    mPlanWindow.Show();
        //}
        private void Screening_Button_Click_1(object sender, RoutedEventArgs e)
        {
            RegisterScreeningView screeningWindow = new RegisterScreeningView();
            screeningWindow.Show();
        }
    }
}
