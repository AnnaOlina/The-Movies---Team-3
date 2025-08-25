using System;
using System.Collections.Generic;
using System.IO;
using The_Movies_WPF_app.Models;

namespace The_Movies_WPF_app.Repositories
{
    public class FileMovieRepository : IMovieRepository
    {
        private readonly string _filePath;

        // Konstruktør
        public FileMovieRepository(string? filePath = null)
        {
            // Gemmer i base directory som "movies.csv"
            var baseDir = AppContext.BaseDirectory;
            _filePath = filePath ?? Path.Combine(baseDir, "movies.csv");

            // Opretter filen hvis den ikke findes
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, string.Empty);
            }
        }

        public IEnumerable<Movie> GetAllMovies()
        {
            // Indlæs linjer
            foreach (var line in File.ReadLines(_filePath))
            {
                yield return Movie.FromString(line);
            }
        }


        public void AddMovie(Movie movie)
        {
            // Tjekker for null-reference
            if (movie is null) throw new ArgumentNullException(nameof(movie));

            // Appenderer ét Movie-objekt til filen
            File.AppendAllText(_filePath, movie.ToString() + Environment.NewLine);
        }
    }
}