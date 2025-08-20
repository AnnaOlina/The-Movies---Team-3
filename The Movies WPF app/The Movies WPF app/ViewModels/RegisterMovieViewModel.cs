using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using The_Movies_WPF_app.Commands;
using The_Movies_WPF_app.Models;
using The_Movies_WPF_app.Helpers;
using The_Movies_WPF_app.Repositories;

namespace The_Movies_WPF_app.ViewModels
{
    public class RegisterMovieViewModel : INotifyPropertyChanged
    {
        // Repository til lagring af film
        private readonly IMovieRepository _movieRepository;

        // Felter
        private string _title;
        private string _durationMinutesText;
        private string _validationMessage;

        // PropertyChanged event til databinding
        public event PropertyChangedEventHandler PropertyChanged;

        // Titel på filmen
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));

                    // Tjekker for dublet-titel
                    if (!string.IsNullOrWhiteSpace(_title) && Movies != null)
                    {
                        ValidationMessage = Movies.Any(m =>
                            string.Equals(m.Title?.Trim(), _title.Trim(), StringComparison.OrdinalIgnoreCase))
                            ? "Der findes allerede en film med denne titel."
                            : string.Empty;
                    }
                    else
                    {
                        ValidationMessage = string.Empty;
                    }

                    // Opdater knapstatus
                    (RegisterMovieCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        // Varighed i minutter (string, for bedre validering)
        public string DurationMinutesText
        {
            get => _durationMinutesText;
            set
            {
                if (_durationMinutesText != value)
                {
                    _durationMinutesText = value;
                    OnPropertyChanged(nameof(DurationMinutesText));
                    (RegisterMovieCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        // Fejlbesked til bruger
        public string ValidationMessage
        {
            get => _validationMessage;
            set
            {
                if (_validationMessage != value)
                {
                    _validationMessage = value;
                    OnPropertyChanged(nameof(ValidationMessage));
                }
            }
        }

        // Liste over tilgængelige genrer (med checkbox)
        public ObservableCollection<GenreItem> AvailableGenres { get; }

        // Liste over registrerede film
        public ObservableCollection<Movie> Movies { get; set; } = new();

        // Kommandoer til knapper
        public ICommand RegisterMovieCommand { get; }
        public ICommand ClearCommand { get; }

        // Constructor
        public RegisterMovieViewModel(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            Movies = new ObservableCollection<Movie>(_movieRepository.GetAllMovies());

            // Initialiser genrer fra enum
            AvailableGenres = new ObservableCollection<GenreItem>(
                Enum.GetValues(typeof(MovieGenre))
                    .Cast<MovieGenre>()
                    .Select(g => new GenreItem { Name = g.ToString(), Genre = g, IsSelected = false })
                    .OrderBy(g => g.Name)
            );

            // Lyt til ændringer i genrevalg
            foreach (var genre in AvailableGenres)
            {
                genre.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(GenreItem.IsSelected))
                    {
                        (RegisterMovieCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    }
                };
            }

            // Initialiser kommandoer
            RegisterMovieCommand = new RelayCommand(RegisterMovie, CanRegisterMovie);
            ClearCommand = new RelayCommand(ClearForm);
        }

        // Validering til registrering af film
        private bool CanRegisterMovie(object parameter)
        {
            bool isValidDuration = int.TryParse(DurationMinutesText, out int minutes) && minutes > 0;

            return !string.IsNullOrWhiteSpace(Title) &&
                   isValidDuration &&
                   AvailableGenres.Any(g => g.IsSelected) &&
                   !Movies.Any(m => string.Equals(m.Title.Trim(), Title.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        // Registrer ny film
        private void RegisterMovie(object parameter)
        {
            int minutes = int.Parse(DurationMinutesText);
            var selectedGenres = AvailableGenres
                .Where(g => g.IsSelected)
                .Select(g => g.Genre)
                .ToList();

            var movie = new Movie(Title, TimeSpan.FromMinutes(minutes), selectedGenres);

            _movieRepository.AddMovie(movie);
            Movies.Add(movie); // vigtigt for validering, hvis man skal tilføje en film mere. Tilføjer film til ObservableCollection.

            ClearForm(null);
        }

        // Ryd formularen
        private void ClearForm(object parameter)
        {
            Title = string.Empty;
            DurationMinutesText = null;
            foreach (var genre in AvailableGenres)
                genre.IsSelected = false;

            (RegisterMovieCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        // Notificer UI om ændringer
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
