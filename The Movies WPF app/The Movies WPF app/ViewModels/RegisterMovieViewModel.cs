using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using The_Movies_WPF_app.Commands;
using The_Movies_WPF_app.Models;
using The_Movies_WPF_app.Repositories;

namespace The_Movies_WPF_app.ViewModels
{
    public sealed class RegisterMovieViewModel : INotifyPropertyChanged
    {
        private readonly IMovieRepository _repo;

        // Bound list in the view
        public ObservableCollection<Movie> Movies { get; } = new();

        // Expose as ICommand (implementation is RelayCommand)
        public ICommand RegisterMovieCommand { get; }

        // Bound form fields
        private string _title = "";
        public string Title
        {
            get => _title;
            set { if (_title != value) { _title = value; OnPropertyChanged(nameof(Title)); } }
        }

        private TimeSpan _runTime;
        public TimeSpan RunTime
        {
            get => _runTime;
            set { if (_runTime != value) { _runTime = value; OnPropertyChanged(nameof(RunTime)); } }
        }

        private MovieGenre _selectedGenre;
        public MovieGenre SelectedGenre
        {
            get => _selectedGenre;
            set { if (_selectedGenre != value) { _selectedGenre = value; OnPropertyChanged(nameof(SelectedGenre)); } }
        }

        // DI happens here: ViewModel knows only the interface
        public RegisterMovieViewModel(IMovieRepository repo)
        {
            _repo = repo;

            // Command wiring
            RegisterMovieCommand = new RelayCommand(_ => RegisterMovie(), _ => CanRegisterMovie());

            // Optional: preload list
            foreach (var m in _repo.GetAllMovies()) Movies.Add(m);
        }

        private bool CanRegisterMovie()
            => !string.IsNullOrWhiteSpace(Title) && RunTime > TimeSpan.Zero;

        // Sequence: RegisterMovie() -> repo.AddMovie(movie) -> ClearFields()
        private void RegisterMovie()
        {
            var movie = new Movie(Title, RunTime, SelectedGenre);

            _repo.AddMovie(movie);     // persistence
            Movies.Add(movie);         // update UI list

            ClearFields();
        }

        private void ClearFields()
        {
            Title = "";
            RunTime = TimeSpan.Zero;
            SelectedGenre = default;
            // Properties already raise OnPropertyChanged in their setters
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
