using The_Movies_WPF_app.Models;
using System.Collections.Generic;

namespace The_Movies_WPF_app.Repositories
{
    public interface IMovieRepository
    {
        // Kontrakt for generering af en sekvens af Movie-objekter
        IEnumerable<Movie> GetAllMovies();
        // Kontrakt for tilføjelse og persistering af ét Movie-objekt
        void AddMovie(Movie movie);
        //Dictionary med MovieId-> RunTime
        Dictionary<Guid, TimeSpan> GetMovieRunTimes();
    }
}