using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieManagementWebApplication.Models;
using MovieManagementWebApplication.Services;

namespace MovieManagementWebApplication.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class AuthorizationControllerForMovieManagement : ControllerBase
{
    private readonly IUserAuthorizationService _authorizationService;

    public AuthorizationControllerForMovieManagement(IUserAuthorizationService service)
    {
        _authorizationService = service;
    }

    [HttpPost("/register")]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterModel model)
    {
        var token = await _authorizationService.RegisterAsync(model);
        return Ok(new { Token = token });
    }

    [HttpPost("/login")]
    public async Task<ActionResult> LoginUser([FromBody] LoginModel model)
    {
        var token = await _authorizationService.LoginAsync(model);
        return Ok(new { Token = token });
    }
}