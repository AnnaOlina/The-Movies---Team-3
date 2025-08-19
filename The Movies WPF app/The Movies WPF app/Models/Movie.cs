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
        public string Title { get; set; }
        public TimeSpan RunTime { get; set; }
        public List<MovieGenre> Genres { get; set; }

        // Konstruktør
        public Movie(string title, TimeSpan runTime, IEnumerable<MovieGenre> genres)
        {
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
            // Splitter hele linjen op i felter adskilt af FieldSeparator
            var parts = line.Split(FieldSeparator);

            var title = parts[0];
            var minutes = int.Parse(parts[1], CultureInfo.InvariantCulture);
            var runTime = TimeSpan.FromMinutes(minutes);
            // Liste af genrer, adskilt af GenreSeparator
            var genres = new List<MovieGenre>();

            foreach (var token in parts[2].Split(GenreSeparator))
            {
                // Parser hver genre fra tekst til enum-værdi
                var g = Enum.Parse<MovieGenre>(token, true);
                genres.Add(g);
            }

            // Opretter og returnerer et nyt Movie-objekt
            return new Movie(title, runTime, genres);
        }
    }
}