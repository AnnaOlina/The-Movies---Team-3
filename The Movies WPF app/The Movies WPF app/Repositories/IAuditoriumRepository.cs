using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies_WPF_app.Repositories
{
    public interface IAuditoriumRepository
    {
        IEnumerable<Auditorium> GetAllAuditoriums();
        void AddAuditorium(Auditorium auditorium);
    }
}
