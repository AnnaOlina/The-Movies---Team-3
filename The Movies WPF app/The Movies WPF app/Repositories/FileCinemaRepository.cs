using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies_WPF_app.Repositories
{
    public class FileCinemaRepository : ICinemaRepository
    {
        private readonly string _filePath;

        //Konstruktør
        public FileCinemaRepository(string? filePath = null)
        {
            //Gemmer i base directory som "cinemas.csv"
            var baseDir = AppContext.BaseDirectory;
            _filePath = filePath ?? Path.Combine(baseDir, "cinemas.csv");

            try
            {
                //Opretter filen hvis den ikke findes
                if (!File.Exists(_filePath))
                {
                    File.WriteAllText(_filePath, string.Empty);
                }
            }
            catch (IOException ex)
            {
                // Håndterer IO-fejl, f.eks. hvis filen ikke kan oprettes
                throw new Exception("Der opstod en fejl under oprettelse af cinemas-filen.", ex);
            }
            catch (Exception ex)
            {
                // Håndterer andre uventede fejl
                throw new IOException($"Kunne ikke oprette eller tilgå filen: {_filePath}", ex);
            }
        }

        public IEnumerable<Cinema> GetAllCinemas()
        {
            // Indlæser linjer én ad gangen og forsøger at konvertere dem til Cinema-objekter
            foreach (var line in File.ReadLines(_filePath))
            {
                // Ignorerer tomme eller whitespace-linjer
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Initialiserer cinema-variablen
                Cinema? cinema = null;

                try
                {
                    // Forsøger at parse linjen til et Cinema-objekt
                    cinema = Cinema.FromString(line);
                }
                catch (FormatException ex)
                {
                    // Håndterer formatfejl, fx hvis linjen ikke har det forventede format
                    throw new Exception($"Fejl ved parsing af cinema-linje: '{line}'", ex);
                }

                // Returnerer cinema-objektet hvis parsing lykkedes
                if (cinema != null)
                    yield return cinema;
            }

        }

        public void AddCinema(Cinema cinema)
        {
            //Tjekker for null-reference
            if (cinema is null) throw new ArgumentNullException(nameof(cinema));

            try
            {
                // Appenderer ét Cinema-objekt til filen
                File.AppendAllText(_filePath, cinema.ToString() + Environment.NewLine);
            }
            catch (IOException ex)
            {
                // Håndterer fejl ved skrivning til filen
                throw new IOException($"Fejl ved skrivning til filen: {_filePath}", ex);
            }
            catch (Exception ex)
            {
                // Håndterer andre uventede fejl
                throw new Exception($"En uventet fejl opstod under tilføjelse af cinema: {cinema.Name}", ex);
            }
        }
    }
}
