using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using The_Movies_WPF_app.Commands;
using The_Movies_WPF_app.Models;
using The_Movies_WPF_app.Repositories;

namespace The_Movies_WPF_app.ViewModels
{
    public class RegisterScreeningViewModel : INotifyPropertyChanged
    {

        /*
         *What do I need:
         *Fill out the view bindings with:
         *Movie dropdown (show available movies by title) List
         *Cinema (show exsiting cinemas to choose from) List
         *StartTime (user manually adds starttime and then the total runtime is calculated behind the scenes with the movie runtime + 30 mins) 
         *Date (user picks a date from a calender, date should only be "selectable" if it has a least 1 auditorium available on that that within that time)
        ^ maybe I should just keep it simple where you add a date manually?
         *Auditorium (user gets auditorium list that changes depending on what cinema is chosen)

           Command for button to add screening to list.
           Command for button to clear fields. (can also be used when you "add" screening, so they're empty when you're done adding)
         */

        // Prøver at bruge samme struktur som fra læringsobjektets eksempel:


        // Trying to put in in order of how it's shown in the view

        // --------------------- 1. Properties

        private readonly IScreeningRepository _screeningRepository;

        private readonly IMovieRepository _movieRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IAuditoriumRepository _auditoriumRepository;


        private Movie _movie;
        public Movie Movie
        {
            get => _movie;
            set
            {
                if (_movie != value)
                {
                    _movie = value;
                    OnPropertyChanged();
                    SaveScreeningCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private Cinema _cinema;
        public Cinema Cinema
        {
            get => _cinema;
            set
            {
                if (_cinema != value)
                {
                    _cinema = value;
                    OnPropertyChanged();
                    UpdateAuditoriums(); // <-- This updates the Auditorium list based on what cinema is chosen.
                    SaveScreeningCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private TimeOnly _startTime;
        public TimeOnly StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateOnly _date;

        public DateOnly Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DateAsDateTime)); // To make DateOnly work with DateTime (calls the wrapper)
                }
            }
        }
        // For DateOnly to work with the kalender, which uses DateTime, we gotta make a "wrapper"
        public DateTime DateAsDateTime
        {
            get => _date.ToDateTime(TimeOnly.MinValue);
            set
            {
                var newDate = DateOnly.FromDateTime(value);
                if (_date != newDate)
                {
                    _date = newDate;
                    OnPropertyChanged(nameof(Date));
                    OnPropertyChanged(nameof(DateAsDateTime));
                }
            }
        }
        //Wrapper over.
        private Auditorium _auditorium;
        public Auditorium Auditorium
        {
            get => _auditorium;
            set { if (_auditorium != value) { _auditorium = value; OnPropertyChanged(); SaveScreeningCommand.RaiseCanExecuteChanged(); } }
        }



        // Constructor
        public RegisterScreeningViewModel()
        {
            // Setting the calender to today's date.
            _date = DateOnly.FromDateTime(DateTime.Today);
            // create repos here
            var movieRepo = new FileMovieRepository();
            var cinemaRepo = new FileCinemaRepository();
            var auditoriumRepo = new FileAuditoriumRepository();
            _screeningRepository = new FileScreeningRepository(movieRepo);
           

            // load data
            Movies = new ReadOnlyObservableCollection<Movie>(
                new ObservableCollection<Movie>(movieRepo.GetAllMovies()));
            Cinemas = new ReadOnlyObservableCollection<Cinema>(
                new ObservableCollection<Cinema>(cinemaRepo.GetAllCinemas()));
            _allAuditoriums = new ObservableCollection<Auditorium>(
                auditoriumRepo.GetAllAuditoriums());
            Auditoriums = new ReadOnlyObservableCollection<Auditorium>(_auditoriums);

            _screenings = new ObservableCollection<Screening>(_screeningRepository.GetAllScreenings());
            Screenings = new ReadOnlyObservableCollection<Screening>(_screenings);

            SaveScreeningCommand = new RelayCommand(_ => SaveScreening(), _ => CanSaveScreening());
            ClearFieldsCommand = new RelayCommand(_ => ClearFields());
        }

        // --------------------- 2. Collections 

        private readonly ObservableCollection<Screening> _screenings = new();
        public ReadOnlyObservableCollection<Screening> Screenings { get; }

        public ReadOnlyObservableCollection<Movie> Movies { get; } // We shouldn't change the list here, just pick from it.
        public ReadOnlyObservableCollection<Cinema> Cinemas { get; } // We shouldn't change the list here, just pick from it.
        public ReadOnlyObservableCollection<Auditorium> Auditoriums { get; }

        
        private readonly ObservableCollection<Auditorium> _allAuditoriums; 
        private readonly ObservableCollection<Auditorium> _auditoriums = new();

        // --------------------- 3. Commands til UI
        public RelayCommand SaveScreeningCommand { get; }
        public RelayCommand ClearFieldsCommand { get; }

        // --------------------- 4. Methods!

        private void ClearFields()
        {
            Movie = null; 
            Cinema = null; 
            StartTime = TimeOnly.MinValue; // Setting it to 00.00.00
            Date = DateOnly.FromDateTime(DateTime.Today); // Should show today.
            Auditorium = null;
        }
        private void UpdateAuditoriums()
        {
            _auditoriums.Clear();
            if (Cinema != null)
            {
                foreach (var aud in _allAuditoriums)
                {
                    if (aud.CinemaId == Cinema.CinemaId)
                    {
                        _auditoriums.Add(aud);
                    }
                }
            }

            Auditorium = null; // reset selection when cinema changes
        }
        private void SaveScreening()
        {
            if (Movie == null || Auditorium == null)
                return;

            // Assume Movie has a TimeSpan property called Runtime
            TimeSpan movieRunTime = Movie.RunTime;

            var newScreening = new Screening(
                screeningId: Guid.NewGuid(),
                date: Date,
                startTime: StartTime,
                runTime: movieRunTime,
                movieId: Movie.MovieId,
                auditoriumId: Auditorium.AuditoriumId
            );

            
            _screeningRepository.AddScreening(newScreening);

            
            _screenings.Add(newScreening);

            System.Windows.MessageBox.Show("Din forestilling er blevet oprettet!", "Sådan!", MessageBoxButton.OK, MessageBoxImage.Information);

            ClearFields();
        }

        private bool CanSaveScreening()
        {
            return Movie != null && Cinema != null && Auditorium != null;
        }
        // --------------------- 5. Property Changed:

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
