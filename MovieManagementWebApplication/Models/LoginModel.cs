using Microsoft.AspNetCore.Identity;

namespace MovieManagementWebApplication.Models;

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}