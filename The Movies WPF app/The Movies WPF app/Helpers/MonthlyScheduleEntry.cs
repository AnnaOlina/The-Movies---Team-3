using System;
using The_Movies_WPF_app.Models;

namespace The_Movies_WPF_app.Helpers
{
    // Denne hjælpeklasse samler information, der skal vises for hver enkelt screening i oversigten.
    public class MonthlyScheduleEntry
    {
        public string MovieTitle { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int AuditoriumNumber { get; set; }
        public DateOnly Date { get; set; }

        public MonthlyScheduleEntry(Screening screening, Movie movie, Auditorium auditorium)
        {
            MovieTitle = movie.Title;
            StartTime = screening.StartTime;
            EndTime = screening.EndTime;
            Duration = movie.RunTime;
            AuditoriumNumber = auditorium.AuditoriumNumber;
            Date = screening.Date;
        }
    }
}