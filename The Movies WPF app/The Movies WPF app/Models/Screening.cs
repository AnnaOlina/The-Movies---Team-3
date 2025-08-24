using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies_WPF_app.Models
{
    public class Screening
    {
        // Definition af separatorformater
        public const char FieldSeparator = ';';
        private TimeSpan movieDuration;
        private TimeOnly movieId;

        // Properties
        public Guid ScreeningId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public Guid MovieId { get; set; }
        public Guid AuditoriumId { get; set; }

        // Konstruktør
        public Screening(Guid screeningId, DateOnly date, TimeOnly startTime, TimeSpan runTime, Guid movieId, Guid auditoriumId)
        {
            if (screeningId == Guid.Empty) throw new ArgumentException("ScreeningId cannot be empty.");
            if (runTime <= TimeSpan.Zero) throw new ArgumentException("Movie duration must be positive.");
            if (movieId == Guid.Empty) throw new ArgumentException("MovieId cannot be empty.");
            if (auditoriumId == Guid.Empty) throw new ArgumentException("AuditoriumId cannot be empty.");

            ScreeningId = screeningId;
            Date = date;
            StartTime = startTime;
            EndTime = startTime.Add(runTime).Add(TimeSpan.FromMinutes(30)); // 15 min reklamer + 15 min rengøring
            MovieId = movieId;
            AuditoriumId = auditoriumId;
        }

        public override string ToString()
        {
            return string.Join(FieldSeparator,
                ScreeningId.ToString("D", CultureInfo.InvariantCulture),
                Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                StartTime.ToString("HH:mm", CultureInfo.InvariantCulture),
                MovieId.ToString("D", CultureInfo.InvariantCulture),
                AuditoriumId.ToString("D", CultureInfo.InvariantCulture));
        }

        public static Screening FromString(string line, TimeSpan runTime)
        {
            if (string.IsNullOrEmpty(line))
                throw new ArgumentNullException(nameof(line), "Input line cannot be null or empty.");
            if (runTime <= TimeSpan.Zero)
                throw new ArgumentException("RunTime must be positive.", nameof(movieDuration));

            var parts = line.Split(FieldSeparator);
            if (parts.Length != 5)
                throw new FormatException("Input line must contain exactly 5 parts: ScreeningId, Date, StartTime, MovieId, AuditoriumId.");
            
            if (!Guid.TryParse(parts[0], out var screeningId))
                throw new FormatException("Invalid ScreeningId format.");
            
            if (!DateOnly.TryParse(parts[1], CultureInfo.InvariantCulture, out var date))
                throw new FormatException("Invalid Date format.");
            
            if (!TimeOnly.TryParse(parts[2], CultureInfo.InvariantCulture, out var startTime))
                throw new FormatException("Invalid StartTime format.");
            
            if (!Guid.TryParse(parts[3], out var movieId))
                throw new FormatException("Invalid MovieId format.");

            if (!Guid.TryParse(parts[4], out var auditoriumId))
                throw new FormatException("Invalid AuditoriumId format.");

            return new Screening(screeningId, date, startTime, runTime, movieId, auditoriumId);
        }
    }
}
