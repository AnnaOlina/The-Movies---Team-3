using System.Collections.ObjectModel;
using System.ComponentModel;
using The_Movies_WPF_app.Models;
using The_Movies_WPF_app.Repositories;
using The_Movies_WPF_app.Helpers;

namespace The_Movies_WPF_app.ViewModels
{
    public class MonthlyScheduleViewModel : INotifyPropertyChanged
    {
        // Repositories til at hente data
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IScreeningRepository _screeningRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IAuditoriumRepository _auditoriumRepository;

        private Cinema _selectedCinema;
        private DateTime _selectedMonth = DateTime.Today;

        public event PropertyChangedEventHandler PropertyChanged;

        // --- Properties til DataBinding ---

        // Liste over alle biografer
        public ObservableCollection<Cinema> Cinemas { get; set; }

        // Den valgte biograf
        public Cinema SelectedCinema
        {
            get => _selectedCinema;
            set
            {
                if (_selectedCinema != value)
                {
                    _selectedCinema = value;
                    OnPropertyChanged(nameof(SelectedCinema));
                    LoadSchedule(); // Opdater kalenderen, når biografen skiftes
                }
            }
        }

        // Den valgte måned
        public DateTime SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value)
                {
                    _selectedMonth = value;
                    OnPropertyChanged(nameof(SelectedMonth));
                    LoadSchedule(); // Opdater kalenderen, når måneden skiftes
                }
            }
        }

        // Den endelige liste af visninger, grupperet efter dato
        public ObservableCollection<Grouping<DateOnly, MonthlyScheduleEntry>> Schedule { get; set; } = new();

        // --- Constructor ---
        public MonthlyScheduleViewModel(ICinemaRepository cinemaRepo, IScreeningRepository screeningRepo, IMovieRepository movieRepo, IAuditoriumRepository auditoriumRepo)
        {
            // Dependency Injection af repositories
            _cinemaRepository = cinemaRepo;
            _screeningRepository = screeningRepo;
            _movieRepository = movieRepo;
            _auditoriumRepository = auditoriumRepo;

            // Hent biografer
            Cinemas = new ObservableCollection<Cinema>(_cinemaRepository.GetAllCinemas());

            // Vælg den første biograf som standard
            if (Cinemas.Any())
            {
                SelectedCinema = Cinemas.First();
            }
        }

        // --- Metoder ---

        private void LoadSchedule()
        {
            // Tjek om der er valgt en biograf
            if (SelectedCinema == null) return;

            // Ryd den gamle tidsplan
            Schedule.Clear();

            // 1. Hent alle data
            var allAuditoriums = _auditoriumRepository.GetAllAuditoriums().ToList();
            var allMovies = _movieRepository.GetAllMovies().ToList();
            var allScreenings = _screeningRepository.GetAllScreenings().ToList();

            // 2. Find de sale, der hører til den valgte biograf
            var cinemaAuditoriums = allAuditoriums.Where(a => a.CinemaId == SelectedCinema.CinemaId).ToList();
            var cinemaAuditoriumIds = cinemaAuditoriums.Select(a => a.AuditoriumId).ToHashSet();

            // 3. Find de visninger, der passer til biografe, måned og år
            var relevantScreenings = allScreenings.Where(s =>
                cinemaAuditoriumIds.Contains(s.AuditoriumId) &&
                s.Date.Year == SelectedMonth.Year &&
                s.Date.Month == SelectedMonth.Month
            ).ToList();

            // 4. Kombiner data til `MonthlyScheduleEntry`-objekter
            var scheduleEntries = from screening in relevantScreenings
                                  join movie in allMovies on screening.MovieId equals movie.MovieId
                                  join auditorium in cinemaAuditoriums on screening.AuditoriumId equals auditorium.AuditoriumId
                                  orderby screening.Date, auditorium.AuditoriumNumber, screening.StartTime
                                  select new MonthlyScheduleEntry(screening, movie, auditorium);

            // 5. Grupper visningerne efter dato
            var groupedByDate = scheduleEntries
                .GroupBy(s => s.Date)
                .Select(g => new Grouping<DateOnly, MonthlyScheduleEntry>(g.Key, g));

            // 6. Opdater den bundne collection
            foreach (var group in groupedByDate)
            {
                Schedule.Add(group);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Lille hjælpeklasse til gruppering
        public class Grouping<TKey, TItem> : ObservableCollection<TItem>
        {
            public TKey Key { get; private set; }
            public Grouping(TKey key, IEnumerable<TItem> items)
            {
                Key = key;
                foreach (var item in items)
                {
                    this.Items.Add(item);
                }
            }
        }
    }
}