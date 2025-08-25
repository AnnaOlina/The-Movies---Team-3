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

        // Properties
        public Guid MovieID { get; }
        public string Title { get; set; }
        public TimeSpan RunTime { get; set; }
        public List<MovieGenre> Genres { get; set; }
        public string Director { get; set; }
        public DateTime PremiereDate { get; set; }

        // Konstruktør
        public Movie(Guid movieID, string title, TimeSpan runTime, List<MovieGenre> genres, string director, DateTime premiereDate)
        {
            MovieID = movieID;
            Title = title;
            RunTime = runTime;
            Genres = genres.ToList();
            Director = director;
            PremiereDate = premiereDate;
        }

        public override string ToString()
        {
            // Lagrer runtime som hele minutter
            var minutes = ((int)Math.Round(RunTime.TotalMinutes, MidpointRounding.AwayFromZero))
                .ToString(CultureInfo.InvariantCulture); // Håndterer decimaltal

            // Saml flere genrer til en string
            var genresJoined = string.Join(GenreSeparator, Genres);

            // Formaterer premieredatoen til "yyyy-MM-dd"
            var premiereDateFormatted = PremiereDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Bygger linje, der skal gemmes i fil
            return string.Join(FieldSeparator,
            MovieID.ToString(),
            Title,
            minutes,
            genresJoined,
            Director,
            premiereDateFormatted);
        }

        public static Movie FromString(string line)
        {
            // Splitter hele linjen op i felter adskilt af FieldSeparator
            var parts = line.Split(FieldSeparator);

            var movieID = Guid.Parse(parts[0]);
            var title = parts[1];
            var minutes = int.Parse(parts[2], CultureInfo.InvariantCulture);
            var runTime = TimeSpan.FromMinutes(minutes);

            // Liste af genrer, adskilt af GenreSeparator
            var genres = parts[3]
            .Split(GenreSeparator)
            .Select(g => Enum.Parse<MovieGenre>(g, true))
            .ToList();

            var director = parts[4];
            var premiereDate = DateTime.ParseExact(parts[5], "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Opretter og returnerer et nyt Movie-objekt
            return new Movie(movieID, title, runTime, genres, director, premiereDate);
        }
    }
}