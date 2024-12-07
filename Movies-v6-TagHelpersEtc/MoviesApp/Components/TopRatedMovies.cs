using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MoviesApp.DataAccess;

namespace MoviesApp.Components
{
    public class TopRatedMovies : ViewComponent
    {
        // Just like controllers, the DI container can inject DB
        // context objects/service objects into out components if needed:
        public TopRatedMovies(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public IViewComponentResult Invoke(int lowestRating, int numberOfMoviesToDisplay)
        {
            // use DB context to get movies with avg rating >= lowestRating:
            var movies = _movieDbContext.Movies
                    .Include(m => m.Genre)
                    .Include(m => m.Reviews)
                    .Where(m => m.Reviews.Average(r => r.Rating).GetValueOrDefault() >= lowestRating)
                    .OrderBy(m => m.Name)
                    .ToList();

            TopRatedMoviesViewModel topRatedMoviesViewModel = new TopRatedMoviesViewModel() { 
                Movies = movies,
                LowestRating = lowestRating,
                NumberOfMoviesToDisplay = numberOfMoviesToDisplay
            };

            return View(topRatedMoviesViewModel);
        }

        private MovieDbContext _movieDbContext;
    }
}
