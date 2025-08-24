using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies_WPF_app.Models
{
    public class Auditorium
    {
        // Definition af separatorformater
        public const char FieldSeparator = ';';

        // Properties
        public Guid AuditoriumId { get; set; }
        public int AuditoriumNumber { get; set; }
        public Guid CinemaId { get; set; }

        // Konstruktør
        public Auditorium(Guid auditoriumId, int auditoriumNumber, Guid cinemaId)
        {
            if (auditoriumId == Guid.Empty) throw new ArgumentException("AuditoriumId cannot be empty.");
            if (auditoriumNumber <= 0) throw new ArgumentException("AuditoriumNumber must be positive.");
            if (cinemaId == Guid.Empty) throw new ArgumentException("CinemaId cannot be empty.");
            AuditoriumId = auditoriumId;
            AuditoriumNumber = auditoriumNumber;
            CinemaId = cinemaId;
        }

        public override string ToString()
        {
            return $"{AuditoriumId}{FieldSeparator}{AuditoriumNumber}{FieldSeparator}{CinemaId}";
        }

        public static Auditorium FromString(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new ArgumentNullException(nameof(line), "Input line cannot be null or empty.");
            
            var parts = line.Split(FieldSeparator);
            if (parts.Length != 3)
                throw new FormatException("Input line must contain exactly three parts: AuditoriumId, AuditoriumNumber, CinemaId.");
            
            if (!Guid.TryParse(parts[0], out var auditoriumId))
                throw new FormatException("Invalid AuditoriumId format.");
            
            if (!int.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var auditoriumNumber) || auditoriumNumber <= 0)
                throw new FormatException("AuditoriumNumber must be a positive integer.");
            
            if (!Guid.TryParse(parts[2], out var cinemaId))
                throw new FormatException("Invalid CinemaId format.");
            
            return new Auditorium(auditoriumId, auditoriumNumber, cinemaId) { AuditoriumId = auditoriumId };
        }
    }
}
