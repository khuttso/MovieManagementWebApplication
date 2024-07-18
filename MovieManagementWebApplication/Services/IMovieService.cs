using MovieManagementWebApplication.Models;

namespace MovieManagementWebApplication.Services;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetAllMovies();
    Task<Movie> GetMovieById(int id);
    Task UpdateMovie(Movie movie);
    Task AddMovie(Movie movie);
    Task DeleteMovie(int id);      
}