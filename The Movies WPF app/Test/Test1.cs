using The_Movies_WPF_app;
using The_Movies_WPF_app.Models;
using The_Movies_WPF_app.ViewModels;
namespace Test
{
    [TestClass]
    public sealed class Test1
    {

        [TestMethod]
        public void RegisterMovieCommand_ShouldAddMovieToList()
        {
            /*
             Testen fra første sprint. Virker ikke, da vi efterfølgende har lavet om på MovieClass :-)

             //Det her er primært fra eksemplet i MVVM læringsprojektet. Den burde bare teste om vores command "RegisterMovieCommand" gør det den skal.
             //Vi skal have en liste af genre i Movie før den kan virke.

            // Arrange

          
            var viewModel = new RegisterMovieViewModel(); //For at lave en "mock" så testen ikke tilføjer en film til den rigtige liste)
            

            viewModel.Title = "Test Movie";
            viewModel.DurationMinutes = 200;
           
           
            // Selecting multiple genres from list: (Horror and Action)
            foreach (var genreItem in viewModel.AvailableGenres)
            {
                if (genreItem.Genre==MovieGenre.Horror)
                genreItem.IsSelected = true;

                if (genreItem.Genre==MovieGenre.Action)
                genreItem.IsSelected = true;
            }


            // Act
            viewModel.RegisterMovieCommand.Execute(null);

            // Assert
            Assert.AreEqual(1, viewModel.Movies.Count);
            var addedMovie = viewModel.Movies[0];
            Assert.AreEqual("Test Movie", addedMovie.Title);
            Assert.AreEqual(200, addedMovie.DurationMinutes);
           
            //with several MovieGenres from a list: CollectionAssert.Contains to checkfor each MovieGenre.
            CollectionAssert.Contains(addedMovie.MovieGenres, MovieGenre.Horror);
            CollectionAssert.Contains(addedMovie.MovieGenres, MovieGenre.Action);

/*
        }
        [TestClass]
        public class RegisterScreeningViewModelTests
        {
            /*[TestMethod]
            public void SaveScreeningCommand_ShouldAddScreening_WhenFieldsAreValid()
            {
                // Arrange
                var viewModel = new RegisterScreeningViewModel();

                // Vi vælger første film og biograf fra listen
                viewModel.Movie = viewModel.Movies[0];
                viewModel.Cinema = viewModel.Cinemas[0];

                // Find et auditorium der matcher biografen
                foreach (var aud in viewModel.Auditoriums)
                {
                    if (aud.CinemaId == viewModel.Cinema.CinemaId)
                    {
                        viewModel.Auditorium = aud;
                        break;
                    }
                }

                viewModel.Date = DateOnly.FromDateTime(DateTime.Today);
                viewModel.StartTime = new TimeOnly(18, 0);

                int initialCount = viewModel.Screenings.Count;

                // Act
                viewModel.SaveScreeningCommand.Execute(null);

                // Assert
                Assert.AreEqual(initialCount + 1, viewModel.Screenings.Count);
            }*/

            [TestMethod]
            public void SaveScreeningCommand_CanExecute_ShouldRespondToFieldCombinations()
            {
                // Arrange fælles testdata
                var testMovie = new Movie(
                    movieID: Guid.NewGuid(),
                    title: "Testfilm",
                    runTime: TimeSpan.FromMinutes(120),
                    genres: new List<MovieGenre> { MovieGenre.Action },
                    director: "Test Instruktør",
                    premiereDate: DateTime.Today.AddDays(-30)
                );

                var testCinemaId = Guid.NewGuid();

                var testCinema = new Cinema(
                    cinemaId: testCinemaId,
                    cinemaName: "TestBio",
                    location: "Aarhus"
                );

                var testAuditorium = new Auditorium(
                    auditoriumId: Guid.NewGuid(),
                    auditoriumNumber: 1,
                    cinemaId: testCinemaId
                );

                var viewModel = new RegisterScreeningViewModel();

                // Case 1: Ingen felter sat
                // Act
                var canExecute1 = viewModel.SaveScreeningCommand.CanExecute(null);
                // Assert
                Assert.IsFalse(canExecute1, "Fejl: Kommandoen burde ikke kunne eksekveres uden felter");

                // Case 2: Kun Movie sat
                viewModel.Movie = testMovie;
                var canExecute2 = viewModel.SaveScreeningCommand.CanExecute(null);
                Assert.IsFalse(canExecute2, "Fejl: Kommandoen burde ikke kunne eksekveres med kun Movie");

                // Case 3: Movie + Cinema sat
                viewModel.Cinema = testCinema;
                var canExecute3 = viewModel.SaveScreeningCommand.CanExecute(null);
                Assert.IsFalse(canExecute3, "Fejl: Kommandoen burde ikke kunne eksekveres uden Auditorium");

                // Case 4: Alle felter sat
                viewModel.Auditorium = testAuditorium;
                var canExecute4 = viewModel.SaveScreeningCommand.CanExecute(null);
                Assert.IsTrue(canExecute4, "Fejl: Kommandoen burde kunne eksekveres når alle felter er sat");
            }
        }
    }
}