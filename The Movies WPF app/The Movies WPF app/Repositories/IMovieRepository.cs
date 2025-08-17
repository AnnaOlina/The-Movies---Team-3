using The_Movies_WPF_app.Models;
using System.Collections.Generic;

namespace The_Movies_WPF_app.Repositories
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAllMovies();
        void AddMovie(Movie movie);
    }
}