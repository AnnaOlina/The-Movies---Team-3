using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using The_Movies_WPF_app.Models;

namespace The_Movies_WPF_app.Repositories
{
    public class FileAuditoriumRepository : IAuditoriumRepository
    {
        private readonly string _filePath;

        //Konstruktør
        public FileAuditoriumRepository(string? filePath = null)
        {
            //Gemmer i base directory som "auditoriums.csv"
            var baseDir = AppContext.BaseDirectory;
            _filePath = filePath ?? Path.Combine(baseDir, "auditoriums.csv");

            try
            {
                // Opretter filen hvis den ikke findes
                if (!File.Exists(_filePath))
                {
                    File.WriteAllText(_filePath, string.Empty);
                }
            }
            catch (IOException ex)
            {
                // Håndterer IO-fejl, f.eks. hvis filen ikke kan oprettes
                throw new IOException($"Der opstod en fejl under oprettelse af auditorium-filen: {_filePath}", ex);
            }
            catch (Exception ex)
            {
                // Håndterer andre uventede fejl
                throw new IOException($"Fejl ved oprettelse af filen: {_filePath}", ex);
            }

        }

        public IEnumerable<Auditorium> GetAllAuditoriums()
        {
            //Indlæs linjer én ad gangen og konverterer til Auditorium-objekter
            foreach (var line in File.ReadLines(_filePath))
            {
                // Ignorerer tomme eller whitespace-linjer
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Initialiserer auditorium-variablen
                Auditorium? auditorium = null;

                try
                {
                    // Forsøger at parse linjen til et Auditorium-objekt
                    auditorium = Auditorium.FromString(line);
                }
                catch (FormatException ex)
                {
                    // Håndterer formatfejl, fx hvis linjen ikke har det forventede format
                    throw new Exception($"Fejl ved parsing af auditorium-linje: '{line}'", ex);
                }

                // Returnerer auditorium-objektet hvis parsing lykkedes
                if (auditorium != null)
                    yield return auditorium;
            }
        }

        public void AddAuditorium(Auditorium auditorium)
        {
            //Tjekker for null-reference
            if (auditorium is null) throw new ArgumentNullException(nameof(auditorium));

            try
            {
                // Appenderer ét Auditorium-objekt til filen
                File.AppendAllText(_filePath, auditorium.ToString() + Environment.NewLine);
            }
            catch (IOException ex)
            {
                // Håndterer fejl ved skrivning til filen, fx hvis filen er låst eller disk fuld
                throw new IOException($"Fejl ved skrivning til filen: {_filePath}", ex);
            }
            catch (Exception ex)
            {
                // Håndterer andre uventede fejl
                throw new Exception($"En uventet fejl opstod under skrivning til auditorium-filen: {_filePath}", ex);
            }
        }
    }
}
