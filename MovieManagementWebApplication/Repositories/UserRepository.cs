namespace MovieManagementWebApplication.Repositories;
using System.Data.SQLite;
using Dapper;
using Microsoft.AspNetCore.Identity;


public class UserRepository : IUserRepository
{   
    /// <summary>
    ///     field for establish database connection
    /// </summary>
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
        InitializeDatabase();
    }

    
    /// <summary>
    ///     InitializeDatabase() - initializes starting configuration for users table, with suitable attributes
    /// </summary>
    private void InitializeDatabase()
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        var createTableCommand =
            @"CREATE TABLE IF NOT EXISTS Users (
                    ID NVARCHAR(100) PRIMARY KEY,
                    UserName NVARCHAR(256) NOT NULL,
                    NormalizedUserName NVARCHAR(256) NOT NULL.
                    Email NVARCHAR(256) NOT NULL,
                    NormalizedEmail NVARCHAR(256) NOT NULL,
                    EmailConfirmed BIT NOT NULL, 
                    PasswordHash NVARCHAR(MAX) NOT NULL
                );
            ";

        using (var command = new SQLiteCommand(createTableCommand, connection))
        {
            command.ExecuteNonQueryAsync();
        }
    }
    
    
    
    
    
    
    /// <summary>
    ///     CreateUserAsync() - stores given user into the database
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns>IdentityResult.Success or IdentityResult.Success depended on execution database command</returns>
    public async Task<IdentityResult> CreateUserAsync(IdentityUser user, string password)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var result =  await connection.ExecuteAsync(
                "INSERT INTO Users (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash)" +
                "VALUES (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash)",
                new
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    NormalizedUserName = user.NormalizedUserName,
                    Email = user.Email,
                    NormalizedEmail = user.NormalizedEmail,
                    EmailConfirmed = user.EmailConfirmed,
                    PasswordHash = user.PasswordHash
                }
            );
            return result == 1 ? IdentityResult.Success : IdentityResult.Failed();
        }
    }

    
    
    
    
    
    /// <summary>
    ///     FindByNameAsync(string name) - searches the user with given name 
    /// </summary>
    /// <param name="name"></param>
    /// <returns>User with given name</returns>
    public async Task<IdentityUser> FindByNameAsync(string name)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryFirstOrDefaultAsync<IdentityUser>(
                "SELECT * FROM Users WHERE NormalizedUserName=@NormalizedUserName",
                new { NormalizedUserName = name }
            );
        }
    }

    
    
    
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns>true if password is correct, false otherwise</returns>
    public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
    {
        return await Task.FromResult(user.PasswordHash == password);
    }
}