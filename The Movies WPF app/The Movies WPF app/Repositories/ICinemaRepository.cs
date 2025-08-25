using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies_WPF_app.Models;

namespace The_Movies_WPF_app.Repositories
{
    public interface ICinemaRepository
    {
        IEnumerable<Cinema> GetAllCinemas();
        void AddCinema(Cinema cinema);
    }
}
