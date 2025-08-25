using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies_WPF_app.Models;

namespace The_Movies_WPF_app.Repositories
{
    public interface IScreeningRepository
    {
        IEnumerable<Screening> GetAllScreenings();
        void AddScreening(Screening screening);
    }
}
