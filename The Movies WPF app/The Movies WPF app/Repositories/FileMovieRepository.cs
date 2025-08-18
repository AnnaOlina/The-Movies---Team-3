using System;
using System.Collections.Generic;
using System.IO;
using The_Movies_WPF_app.Models;

namespace The_Movies_WPF_app.Repositories
{
    public class FileMovieRepository : IMovieRepository
    {
        private readonly string _filePath;

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
            // Indlæs linjer. Spring over tomme linjer.
            foreach (var line in File.ReadLines(_filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Forsøger at konvertere linjen til et Movie-objekt via Movie.FromString
                Movie? movie = null;
                try
                {
                    movie = Movie.FromString(line);
                }
                catch
                {
                    continue; // Forkert format = Linje springes over
                }

                if (movie != null)
                    yield return movie;
            }
        }

        public void AddMovie(Movie movie)
        {
            if (movie is null) throw new ArgumentNullException(nameof(movie));

            // Appenderer ét Movie-objekt til hukommelsen. 
            File.AppendAllText(_filePath, movie.ToString() + Environment.NewLine);
        }
    }
}