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

namespace The_Movies_WPF_app.ViewModels
{
    public class RegisterMovieViewModel : INotifyPropertyChanged
    {
        // Repository til lagring af film
        private readonly IMovieRepository _movieRepository;

        // Felter
        private string _title;
        private int _durationMinutes;
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

        // Varighed i minutter
        public int DurationMinutes
        {
            get => _durationMinutes;
            set
            {
                if (_durationMinutes != value)
                {
                    _durationMinutes = value;
                    OnPropertyChanged(nameof(DurationMinutes));
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
            return !string.IsNullOrWhiteSpace(Title) &&
                   DurationMinutes > 0 &&
                   AvailableGenres.Any(g => g.IsSelected) &&
                   !Movies.Any(m => string.Equals(m.Title.Trim(), Title.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        // Registrer ny film
        private void RegisterMovie(object parameter)
        {
            var selectedGenres = AvailableGenres
                .Where(g => g.IsSelected)
                .Select(g => g.Genre)
                .ToList();

            var movie = new Movie(Title, TimeSpan.FromMinutes(DurationMinutes), selectedGenres);

            _movieRepository.AddMovie(movie);
            Movies.Add(movie); // vigtigt for validering, hvis man skal tilføje en film mere. Tilføjer film til ObservableCollection.

            ClearForm(null);
        }

        // Ryd formularen
        private void ClearForm(object parameter)
        {
            Title = string.Empty;
            DurationMinutes = 0;
            foreach (var genre in AvailableGenres)
                genre.IsSelected = false;

            (RegisterMovieCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        // Notificer UI om ændringer
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Hjælpeklasse til genrevalg
    public class GenreItem : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string Name { get; set; }
        public MovieGenre Genre { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
