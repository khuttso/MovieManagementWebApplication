using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieManagementWebApplication.Models;
using MovieManagementWebApplication.Services;

namespace MovieManagementWebApplication.Controllers;

[Authorize]
[ApiController]
[Route("/api/Movies")]
public class MovieController : ControllerBase
{
    
    private readonly IMovieService _movieService;
    private readonly ILogger<MovieController> _logger;
    
    public MovieController(IMovieService movieService, ILogger<MovieController> logger)
    {
        _movieService = movieService;
        _logger = logger;
    }

    [HttpGet("/GetAllMovies")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
    {
        var movies = await _movieService.GetAllMovies();
        return Ok(movies);
    }

    [HttpGet("/GetMovieById")]
    public async Task<ActionResult<Movie>> GetMovieById(int id)
    {
        var movie = await _movieService.GetMovieById(id);
        return Ok(movie);
    }

    [HttpPost("/AddMovie")]
    public async Task<ActionResult> AddMovie(Movie movie)
    {
        await _movieService.AddMovie(movie);
        return Ok();
    }

    [HttpPut("/UpdateMovie")]
    public async Task<ActionResult> UpdateMovie(int id, Movie movie)
    {
        if (id != movie.Id)
        {
            return BadRequest();
        }
        await _movieService.UpdateMovie(movie);
        return Ok();
    }

    [HttpDelete("/DeleteMovie")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        await _movieService.DeleteMovie(id);
        return Ok();
    }
}