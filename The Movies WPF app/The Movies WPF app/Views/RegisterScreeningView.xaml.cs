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
using System.Windows.Shapes;
using The_Movies_WPF_app.Repositories;
using The_Movies_WPF_app.ViewModels;

namespace The_Movies_WPF_app.Views
{
    /// <summary>
    /// Interaction logic for RegisterScreeningView.xaml
    /// </summary>
    public partial class RegisterScreeningView : Window
    {
        public RegisterScreeningView()
        {
            InitializeComponent();
           // DataContext = new RegisterScreeningViewModel(new FileScreeningRepository("MonthlyPlan.csv"));
        }
    }
}
