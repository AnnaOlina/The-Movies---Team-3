using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using The_Movies_WPF_app.Commands;
using The_Movies_WPF_app.Models;
using The_Movies_WPF_app.Helpers;
using The_Movies_WPF_app.Repositories;

namespace The_Movies_WPF_app.ViewModels
{
    public class RegisterMovieViewModel : INotifyPropertyChanged
    {
        // Repository (Modtages via DI)
        private readonly IMovieRepository _movieRepository;

        // Felter der understøtter UI-properties
        private string _title;
        private string _durationMinutesText;
        private string _validationMessage;
        private string _director;
        private DateTime _premiereDate = DateTime.Today;

        public event PropertyChangedEventHandler PropertyChanged;

        // --- Properties til DataBinding ---

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
                    (RegisterMovieCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

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

        public string Director
        {
            get => _director;
            set
            {
                if (_director != value)
                {
                    _director = value;
                    OnPropertyChanged(nameof(Director));
                    (RegisterMovieCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public DateTime PremiereDate
        {
            get => _premiereDate;
            set
            {
                if (_premiereDate != value)
                {
                    _premiereDate = value;
                    OnPropertyChanged(nameof(PremiereDate));
                    (RegisterMovieCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        // Collection til CheckBox-listen i UI'en
        public ObservableCollection<GenreItem> AvailableGenres { get; }

        // Liste over eksisterende film (til validering)
        public ObservableCollection<Movie> Movies { get; set; } = new();

        // Commands (som XAML binder til)
        public ICommand RegisterMovieCommand { get; }
        public ICommand ClearCommand { get; }

        // Constructor (Modtager IMovieRepository fra DI Container)
        public RegisterMovieViewModel(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            Movies = new ObservableCollection<Movie>(_movieRepository.GetAllMovies());

            // Initialiser genrer fra Enum
            AvailableGenres = new ObservableCollection<GenreItem>(
                Enum.GetValues(typeof(MovieGenre))
                    .Cast<MovieGenre>()
                    .Select(g => new GenreItem { Name = g.ToString(), Genre = g, IsSelected = false })
                    .OrderBy(g => g.Name)
            );

            // Lyt til ændringer i checkbox-valg
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

        private bool CanRegisterMovie(object parameter)
        {
            bool isValidDuration = int.TryParse(DurationMinutesText, out int minutes) && minutes > 0;

            return !string.IsNullOrWhiteSpace(Title) &&
                    !string.IsNullOrWhiteSpace(Director) &&
                    isValidDuration &&
                    AvailableGenres.Any(g => g.IsSelected) &&
                    !Movies.Any(m => string.Equals(m.Title.Trim(), Title.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private void RegisterMovie(object parameter)
        {
            // 1. Hent data fra properties
            int minutes = int.Parse(DurationMinutesText);

            // 2. Konverter de valgte GenreItems til den List<MovieGenre>, som constructoren kræver
            var selectedGenres = AvailableGenres
                .Where(g => g.IsSelected)
                .Select(g => g.Genre)
                .ToList();

            // 3. Kald constructoren med 6 PÅKRÆVEDE ARGUMENTER
            var movie = new Movie(
                Guid.NewGuid(),                 // Argument 1: MovieId
                Title,                          // Argument 2: Title
                TimeSpan.FromMinutes(minutes),  // Argument 3: RunTime
                selectedGenres,                 // Argument 4: List<MovieGenre>
                Director,                       // Argument 5: Director
                PremiereDate);                  // Argument 6: PremiereDate

            _movieRepository.AddMovie(movie);
            Movies.Add(movie); // Opdaterer listen til dublet-validering

            ClearForm(null);
        }

        private void ClearForm(object parameter)
        {
            Title = string.Empty;
            DurationMinutesText = null;
            foreach (var genre in AvailableGenres)
                genre.IsSelected = false;

            Director = string.Empty;
            PremiereDate = DateTime.Today;

            (RegisterMovieCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        // Standard INotifyPropertyChanged implementering
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}