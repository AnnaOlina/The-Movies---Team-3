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
            // Default fildestination: <AppBase>/Data/movies.csv
            var baseDir = AppContext.BaseDirectory;
            var dataDir = Path.Combine(baseDir, "Data");
            Directory.CreateDirectory(dataDir);

            _filePath = filePath ?? Path.Combine(dataDir, "movies.csv");

            // Tjekker om fil findes. Hvis ikke, oprettes tom fil
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

            // Appenderer ét Movie-objekt til hukommelsen
            File.AppendAllText(_filePath, movie.ToString() + Environment.NewLine);
        }
    }
}