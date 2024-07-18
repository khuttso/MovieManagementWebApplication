using Microsoft.AspNetCore.Identity;

namespace MovieManagementWebApplication.Repositories;

public interface IUserRepository
{
    Task<IdentityResult> CreateUserAsync(IdentityUser user, string password);
    Task<IdentityUser> FindByNameAsync(string name);
    Task<bool> CheckPasswordAsync(IdentityUser user, string password);
    
}