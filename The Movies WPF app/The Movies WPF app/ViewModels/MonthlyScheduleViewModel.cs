using System.Collections.ObjectModel; // Adgang til ObservableCollection<T>
using System.ComponentModel; // Adgang til INotifyPropertyChanged
using System.Windows.Input; // Adgang til ICommand
using The_Movies_WPF_app.Models;

namespace The_Movies_WPF_app.ViewModels
{
    public class MonthlyScheduleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // --- Properties til Visning og Valg ---

        // Listen over filmvisninger, der skal vises i skemaet.
        public ObservableCollection<Screening> Screenings { get; set; }

        // Listen over biografer til dropdown-menuen.
        public List<Cinema> AllCinemas { get; set; }

        // Den biograf, brugeren har valgt i dropdown-menuen.
        // Når denne ændres, skal vi indlæse programmet.
        public Cinema SelectedCinema { get; set; }

        // --- Kommandoer ---

        // Kommando til at indlæse programmet for den valgte biograf.
        public ICommand LoadScreeningsCommand { get; }

        // --- Konstruktør ---
        public MonthlyScheduleViewModel()
        {
            // Initialiser listerne
            Screenings = new ObservableCollection<Screening>();
            AllCinemas = new List<Cinema>();

            // Her skal logikken for kommandoen og dataindlæsning tilføjes.
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}