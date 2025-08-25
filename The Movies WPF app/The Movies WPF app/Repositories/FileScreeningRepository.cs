using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using The_Movies_WPF_app.Models;

namespace The_Movies_WPF_app.Repositories
{
    public class FileScreeningRepository : IScreeningRepository
    {
        private readonly string _filePath;
        private readonly IMovieRepository _movieRepository;

        //Konstruktør
        public FileScreeningRepository(IMovieRepository movieRepository, string? filePath = null)
        {
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));

            var baseDir = AppContext.BaseDirectory;
            _filePath = filePath ?? Path.Combine(baseDir, "screenings.csv");

            try
            {
                if (!File.Exists(_filePath))
                {
                    File.WriteAllText(_filePath, string.Empty);
                }
            }
            catch (IOException ex)
            {
                // Håndterer IO-fejl, f.eks. hvis filen ikke kan oprettes
                throw new Exception("Der opstod en fejl under oprettelse af screenings-filen.", ex);
            }
            catch (Exception ex)
            {
                // Håndterer andre uventede fejl
                throw new Exception("En uventet fejl opstod under initialisering af screenings-filen.", ex);
            }
        }

        public IEnumerable<Screening> GetAllScreenings()
        {
            //Henter et Dictionary af MovieId og RunTime fra MovieRepository
            var movieRunTimes = _movieRepository.GetMovieRunTimes();

            //Indlæser linjer én ad gangen og konverterer til Screening-objekter
            foreach (var line in File.ReadLines(_filePath))
            {
                // Ignorerer tomme eller whitespace-linjer
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Splitter linjen og tjekker om den har det forventede antal felter og at movieId er gyldigt
                var parts = line.Split(Screening.FieldSeparator);
                if (parts.Length != 5 || !Guid.TryParse(parts[3], out var movieId))
                    continue;

                // Henter runTime for den givne movieId, hvis den findes
                if (!movieRunTimes.TryGetValue(movieId, out var runTime))
                    continue;

                Screening? screening = null;

                try
                {
                    //Opretter Screening-objektet med runtime
                    screening = Screening.FromString(line, runTime);
                }
                catch (FormatException ex)
                {
                    // Håndterer formatfejl, fx hvis linjen ikke har det forventede format
                    throw new Exception($"Fejl ved parsing af screening-linje: '{line}'", ex);
                }
                // Returnerer screening-objektet hvis parsing lykkedes
                if (screening != null)
                    yield return screening;
            }
        }


        public void AddScreening(Screening screening)
        {
            //Tjekker for null-reference
            if (screening is null) throw new ArgumentNullException(nameof(screening));

            try
            {
                //Appenderer ét Screening-objekt til filen
                File.AppendAllText(_filePath, screening.ToString() + Environment.NewLine);
            }
            catch (IOException ex)
            {
                // Håndterer IO-fejl, f.eks. hvis filen er låst eller der er skriveproblemer
                throw new Exception("Der opstod en fejl under skrivning til screenings-filen.", ex);
            }
            catch (Exception ex)
            {
                // Håndterer andre uventede fejl
                throw new Exception("En uventet fejl opstod under tilføjelse af screening.", ex);
            }
        }
    }
}
