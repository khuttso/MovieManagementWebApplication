using Microsoft.AspNetCore.Mvc;
using MovieManagementWebApplication.Models;

namespace MovieManagementWebApplication.Services;

public interface IUserAuthorizationService
{
    Task<string> RegisterAsync(RegisterModel registerModel);
    Task<string> LoginAsync(LoginModel loginModel);
}   