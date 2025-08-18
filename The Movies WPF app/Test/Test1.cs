using The_Movies_WPF_app;
using The_Movies_WPF_app.Models;
namespace Test
{
    [TestClass]
    public sealed class Test1
    {
       
        [TestMethod]
        public void RegisterMovieCommand_ShouldAddMovieToList()
        {
            /*
             //Det her er primært fra eksemplet i MVVM læringsprojektet. Den burde bare teste om vores command "RegisterMovieCommand" gør det den skal.
             //Vi skal have en liste af genre i Movie før den kan virke.

            // Arrange

          
            var viewModel = new RegisterMovieViewModel(new FakeMovieRepository()); //For at lave en "mock" så testen ikke tilføjer en film til den rigtige liste)
            

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

*/
        }
    }
}
