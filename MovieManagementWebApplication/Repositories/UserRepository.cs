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
        // var dropTableCommand = @"DROP TABLE IF EXISTS Users";
        // using (var command = new SQLiteCommand(dropTableCommand))
        // {
        //     command.ExecuteNonQuery();
        // }
        
        
        var createTableCommand =
            @"CREATE TABLE IF NOT EXISTS Users (
                    ID NVARCHAR(40) PRIMARY KEY,
                    UserName NVARCHAR(40) NOT NULL,
                    Email NVARCHAR(40) NOT NULL,
                    EmailConfirmed BIT NOT NULL, 
                    PasswordHash TEXT NOT NULL
                )
            ";

        using (var command = new SQLiteCommand(createTableCommand, connection))
        {
            command.ExecuteNonQuery();
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
            var hasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = hasher.HashPassword(user, password);
            user.NormalizedEmail = user.Email.ToUpper();
            user.NormalizedUserName = user.UserName.ToUpper();
            
            var result =  await connection.ExecuteAsync(
                "INSERT INTO Users (Id, UserName, Email, EmailConfirmed, PasswordHash)" +
                "VALUES (@Id, @UserName, @Email, @EmailConfirmed, @PasswordHash)",
                new
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
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
                "SELECT * FROM Users WHERE UserName=@UserName",
                new { UserName = name }
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
        PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success;
    }
}