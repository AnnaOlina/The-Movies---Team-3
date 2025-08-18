using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace The_Movies_WPF_app.Models
{
    // Enum bruges direkte i strengformatet med sine navne (Action|Drama|...)
    public enum MovieGenre
    {
        Action,
        Comedy,
        Drama,
        Horror,
        SciFi,
        Romance,
        Thriller,
        Fantasy,
        Animation,
        Documentary,
        Other
    }

    public class Movie
    {
        // Definition af separatorformater
        public const char FieldSeparator = ';';
        public const char GenreSeparator = '|';

        // Domænefelter
        public string Title { get; set; }
        public TimeSpan RunTime { get; set; }
        public List<MovieGenre> Genres { get; set; }

        // Konstruktør
        public Movie(string title, TimeSpan runTime, IEnumerable<MovieGenre> genres)
        {
            // Validering af felter
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title må ikke være tom.", nameof(title));
            if (runTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(runTime), "RunTime må ikke være negativ.");
            if (genres is null)
                throw new ArgumentNullException(nameof(genres), "Genres må ikke være null.");

            Title = title;
            RunTime = runTime;
            Genres = genres.ToList();
        }

        // Tillader at angive én eller flere genrer
        public Movie(string title, TimeSpan runTime, params MovieGenre[] genres)
            : this(title, runTime, (IEnumerable<MovieGenre>)genres) { }

        public override string ToString()
        {
            // Lagrer runtime som hele minutter
            var minutes = ((int)Math.Round(RunTime.TotalMinutes, MidpointRounding.AwayFromZero))
                          .ToString(CultureInfo.InvariantCulture); // Håndterer decimaltal

            // Saml flere genrer til en string
            var genresJoined = string.Join(GenreSeparator, Genres);

            // Bygger linje, der skal gemmes i fil
            return string.Join(FieldSeparator, Title, minutes, genresJoined);
        }

        public static Movie FromString(string line)
        {
            // Validering af format
            if (string.IsNullOrWhiteSpace(line))
                throw new ArgumentException("Linjen er tom.", nameof(line));

            var parts = line.Split(FieldSeparator);
            if (parts.Length < 3)
                throw new FormatException("Forventet format: Title;RunTimeMinutes;Genres");

            var titleRaw = parts[0].Trim();
            if (string.IsNullOrWhiteSpace(titleRaw))
                throw new FormatException("Title må ikke være tom.");

            if (!int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var minutes))
                throw new FormatException("RunTimeMinutes skal være et heltal.");

            var runTime = TimeSpan.FromMinutes(minutes);

            // Parse genrer (skal indeholde mindst én)
            var genres = new List<MovieGenre>();
            var genresCell = parts[2];

            if (string.IsNullOrWhiteSpace(genresCell))
                throw new FormatException("Genres-feltet må ikke være tomt.");

            foreach (var token in genresCell.Split(GenreSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (!Enum.TryParse<MovieGenre>(token, ignoreCase: true, out var g))
                    throw new FormatException($"Ukendt genre: {token}");
                genres.Add(g);
            }

            if (genres.Count == 0)
                throw new FormatException("Genres-feltet skal indeholde mindst én gyldig genre.");

            return new Movie(titleRaw, runTime, genres);
        }
    }
}