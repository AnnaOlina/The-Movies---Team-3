using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies_WPF_app.Models
{
    public enum MovieGenre
    {
        Action,
        Comedy,
        Drama,
        Horror,
        SciFi,
        Romance,
        Documentary,
        Other
    }

    internal class Movie
    {
        public string Title { get; set; }
        public TimeSpan RunTime { get; set; }
        public MovieGenre Genre { get; set; }

        public Movie(string title, TimeSpan runTime, MovieGenre genre)
        {
            Title = title;
            RunTime = runTime;
            Genre = genre;
        }

        public override string ToString()
        {
            return $"{Title} ({RunTime.Hours}h {RunTime.Minutes}m, {Genre})";
        }
    }
}
