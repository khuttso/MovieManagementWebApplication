using MovieManagementWebApplication.Models;
using MovieManagementWebApplication.Repositories;

namespace MovieManagementWebApplication.Services;

public class MovieService : IMovieService
{
    /// <summary>
    ///     IMovieRepository field for database operations
    /// </summary>
    private readonly IMovieRepository _repository;

    public MovieService(IMovieRepository repository)
    {
        _repository = repository;
    }
    
    
    /// <summary>
    ///     All the methods below calling suitable methods of the repository object.
    /// </summary>
    
    public async Task<IEnumerable<Movie>> GetAllMovies()
    {
        return await _repository.GetAllMoviesAsync();
    }

    public async Task<Movie> GetMovieById(int id)
    {
        return await _repository.GetMovieById(id);
    }

    public async Task UpdateMovie(Movie movie)
    {
        await _repository.UpdateMovieAsync(movie);
    }

    public async Task AddMovie(Movie movie)
    {
        await _repository.AddMovieAsync(movie);
    }

    public async Task DeleteMovie(int id)
    {
        var movie = await GetMovieById(id);
        await _repository.RemoveMovieAsync(movie);
    }
}