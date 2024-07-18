using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieManagementWebApplication.Models;
using MovieManagementWebApplication.Repositories;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MovieManagementWebApplication.Services;


public class UserAuthorizationServiceUsingJwt : IUserAuthorizationService
{
    /// <summary>
    ///   Attributes: UserManager<IdentityUser>, IConfiguration, UserAuthorizationServiceUsingJwt
    ///     
    /// </summary>


    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;

    public UserAuthorizationServiceUsingJwt(IConfiguration configuration, IUserRepository userRepository)
    {
        _repository = userRepository;
        _configuration = configuration;
        _secretKey = _configuration["Jwt:Key"];
        _issuer = _configuration["Jwt:Issuer"];
    }
    
    public async Task<string> RegisterAsync(RegisterModel registerModel)
    {
        var user = new IdentityUser()
        {
            UserName = registerModel.Username,
            Email = registerModel.Email,
            NormalizedUserName = registerModel.Username.ToUpper(),
            NormalizedEmail = registerModel.Email.ToUpper(),
            EmailConfirmed = true,
            PasswordHash = registerModel.Password
        };
        var result = await _repository.CreateUserAsync(user, registerModel.Password);
        if (result.Succeeded)
        {
            return GenerateJwtToken(user);
        }

        throw new Exception("User registration failed");
    }

    public async Task<string> LoginAsync(LoginModel loginModel)
    {
        var user = await _repository.FindByNameAsync(loginModel.Username.ToUpper());
        if (await _repository.CheckPasswordAsync(user, loginModel.Password))
        {
            return GenerateJwtToken(user);
        }

        throw new Exception("Invalid login attempt");
    }


    /// <summary>
    ///     var tokenHandler = new JwtSecurityTokenHandler(); for handling jwt
    ///     var key = Encoding.ASCII.GetBytes(_secretKey);  converting bytes array for signing
    ///
    ///
    ///     var tokenDescriptor = new SecurityTokenDescriptor()
    ///      {
    ///         Subject = new ClaimsIdentity(new Claim[]
    ///     {
    ///        // claim user id and name
    ///        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    ///        new Claim(ClaimTypes.Name, user.UserName)
    ///    }),
    /// 
    ///    this means that token will be expired in 0.5 hours
    ///    Expires = DateTime.UtcNow.AddHours(0.5),
    ///    Issuer = _issuer,
    ///        
    ///    // some algorithm named HmacSha256Signature
    ///    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    ///};
    /// </summary>
    /// <param name="user"></param>
    /// <returns>token as a string</returns>
    private string GenerateJwtToken(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                // claim user id and name
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            }),

            Expires = DateTime.UtcNow.AddHours(0.5),
            Issuer = _issuer,
            
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}