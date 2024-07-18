namespace MovieManagementWebApplication.Repositories;
using MovieManagementWebApplication.Models;
public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAllMoviesAsync();
    Task<Movie> GetMovieById(int id);
    Task AddMovieAsync(Movie movie);
    Task RemoveMovieAsync(Movie movie);
    Task UpdateMovieAsync(Movie movie); 
}