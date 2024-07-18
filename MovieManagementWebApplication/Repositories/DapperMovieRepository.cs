using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using Dapper;
using MovieManagementWebApplication.Models;

namespace MovieManagementWebApplication.Repositories;
/// <summary>
///     Class <c>DapperMovieRepository</c> for database operations for Movie data
/// </summary>
public class DapperMovieRepository : IMovieRepository
{
    /// <summary>
    ///     IDbConnection field for database connection
    /// </summary>
    private readonly IDbConnection _dbConnection;

    public DapperMovieRepository(IDbConnection dbConnection)
    {
        _dbConnection = new SqlConnection();
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        var sqlCommand =
            @"CREATE TABLE IF NOT EXIST Movies (
                Id INT PRIMARY KEY,
                Title TEXT NOT NULL,
                Director TEXT NOT NULL,
                ReleaseYear INT,
                Genre TEXT
            )";

        using (var command = new SQLiteCommand(sqlCommand, (SQLiteConnection) _dbConnection))
        {
            command.ExecuteNonQuery();
        }        
    }
    
    
    
    
    
    ///<summary>Selects all the data from database</summary>
    /// <returns>IEnumerable<Movie>> all the movies in the database</returns>
    public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
    {
        var sqlCommand = "SELECT * FROM Movies";
        return await _dbConnection.QueryAsync<Movie>(sqlCommand);
    }
    
    
    
    
    /// <summary>
    ///     Selects movie from Movies table that has given id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Movie with given id</returns>
    public async Task<Movie> GetMovieById(int id)
    {
        var sqlCommand = "SELECT * FROM Movies WHERE Id = @Id";
        return await _dbConnection.QueryFirstOrDefaultAsync<Movie>(sqlCommand, 
            new
            {
                id
            });
    }

    
    
    
    
    /// <summary>
    ///     Adds given movie into the database
    /// </summary>
    /// <param name="movie"></param>
    public async Task AddMovieAsync(Movie movie)
    {
        var sqlCommand = "INSERT INTO Movies (Id, Title, Director, Genre, ReleaseDate) VALUES (@Id, @Title, @Director, @ReleaseDate, @Genre)";
        await _dbConnection.QueryAsync<Movie>(sqlCommand,
            new
            {
                movie.Id,
                movie.Title,
                movie.Director,
                movie.ReleaseYear,
                movie.Genre
            });
    }

    
    
    
    
    
    /// <summary>
    ///     Removes given movie from the database
    /// </summary>
    /// <param name="movie"></param>
    public async Task RemoveMovieAsync(Movie movie)
    {
        var sqlCommand = "DELETE FROM Movies WHERE Id = @Id";
        await _dbConnection.QueryAsync<Movie>(sqlCommand,
            new
            {
                movie.Id
            });
    }

    
    
    
    
    
    /// <summary>
    ///     updates movie with id movie.id with sets given attributes
    /// </summary>
    /// <param name="movie"></param>
    public async Task UpdateMovieAsync(Movie movie)
    {
        int id = movie.Id;
        var sqlCommand =
            "UPDATE TABLE Movies SET Title = @Title, Director = @Director, ReleaseYear = @ReleaseYear, Genre = @Genre WHERE Id = @Id";
        await _dbConnection.QueryAsync<Movie>(sqlCommand, movie);
    }
}