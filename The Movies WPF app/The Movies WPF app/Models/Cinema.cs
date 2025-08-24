using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies_WPF_app.Models
{
    public class Cinema
    {
        // Definition af separatorformater
        public const char FieldSeparator = ';';

        // Properties
        public Guid CinemaId { get; set; }
        public string CinemaName { get; set; }
        public string Location { get; set; }

        public Cinema(Guid cinemaId, string cinemaName, string location)
        {
            if (cinemaId == Guid.Empty) throw new ArgumentException("CinemaId cannot be empty.");
            if (string.IsNullOrWhiteSpace(cinemaName)) throw new ArgumentException("CinemaName cannot be empty.");
            if (string.IsNullOrWhiteSpace(location)) throw new ArgumentException("Location cannot be empty.");
            CinemaId = cinemaId;
            CinemaName = cinemaName;
            Location = location;
        }

        public override string ToString()
        {
            return string.Join(FieldSeparator, CinemaId.ToString("D", CultureInfo.InvariantCulture),
                CinemaName, Location);
        }

        public static Cinema FromString(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new ArgumentNullException(nameof(line), "Input line cannot be null or empty.");

            var parts = line.Split(FieldSeparator);
            if (parts.Length != 3)
                throw new FormatException("Input line must contain exactly three parts: CinemaId, CinemaName, Location.");

            if (!Guid.TryParse(parts[0], out var cinemaId))
                throw new FormatException("Invalid CinemaId format.");

            var cinemaName = parts[1].Trim();
            var location = parts[2].Trim();

            if (string.IsNullOrWhiteSpace(cinemaName) || string.IsNullOrWhiteSpace(location))
            {
                throw new FormatException("CinemaName and Location cannot be empty.");
            }

            return new Cinema(cinemaId, cinemaName, location);
        }
    }
}
