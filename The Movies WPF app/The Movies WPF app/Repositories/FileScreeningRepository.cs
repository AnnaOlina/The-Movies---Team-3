using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace The_Movies_WPF_app.Repositories
{
    public class FileScreeningRepository : IScreeningRepository
    {
        private readonly string _filePath;

        //Konstruktør
        public FileScreeningRepository(string? filePath = null)
        {
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
            //Indlæs linjer én ad gangen og forsøger at konvertere til Screening-objekter
            foreach (var line in File.ReadLines(_filePath))
            {
                // Ignorerer tomme eller whitespace-linjer
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Initialiserer screening-variablen
                Screening? screening = null;

                try
                {
                    //Forsøger at parse linjen til et Screening-objekt
                    screening = Screening.FromString(line);
                }
                catch (FormatException ex)
                {
                    // Håndterer formatfejl, f.eks. hvis linjen ikke kan parses korrekt
                    throw new Exception($"Fejl ved parsing af screening-linje: '{line}'", ex);
                }
                //Returnerer screening-objektet, hvis parsin lykkedes.
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
