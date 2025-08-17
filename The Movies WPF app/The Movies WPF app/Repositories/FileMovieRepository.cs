using System.IO;
using The_Movies_WPF_app.Models;

namespace The_Movies_WPF_app.Repositories
{
    // Definerer offentlig klasse, som overholder IMovieRepository
    public class FileMovieRepository : IMovieRepository
    {
        // Opretter en privat variabel _filePath, som er 'readonly' (værdi kan kun tildeles én gang)
        private readonly string _filePath;
        // Konstruktør, der kører, når et nyt objekt af klassen skabes, om som definerer filstien
        public FileMovieRepository()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _filePath = Path.Combine(documentsPath, "movies.txt");
        }

        public void AddMovie(Movie movie)
        {
            string lineToWrite = movie.ToString() + Environment.NewLine;
            File.AppendAllText(_filePath, lineToWrite);
        }
        // Metode, der henter alle Movie-objekter
        public IEnumerable<Movie> GetAllMovies()
        {
            // Hvis filen ikke eksisterer, skabes og returneres tom liste
            if (!File.Exists(_filePath))
            {
                return Enumerable.Empty<Movie>();
            }

            string[] movieLines = File.ReadAllLines(_filePath);
            var movies = new List<Movie>();

            foreach (string line in movieLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                Movie movie = Movie.FromString(line);
                movies.Add(movie);
            }
            return movies;
        }
    }
}
