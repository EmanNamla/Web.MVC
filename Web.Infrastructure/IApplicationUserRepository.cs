using Microsoft.AspNetCore.Identity;

namespace Web.Core.Interfaces.Identity
{
    public interface IApplicationUserRepository
    {
        Task<IdentityResult> CreateUserAsync(ApplicationUser? user, string password);
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<bool> CheckPassword(ApplicationUser? user, string password);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<SignInResult> PasswordSignInAsync(ApplicationUser? user, string password);
    }
}
