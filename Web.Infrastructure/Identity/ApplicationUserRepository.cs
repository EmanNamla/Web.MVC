using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Core.Interfaces.Identity;

namespace Web.Infrastructure.Identity
{
    public class ApplicationUserRepository:IApplicationUserRepository
    {
        private readonly UserManager<ApplicationUser?> _userManager;
        private readonly SignInManager<ApplicationUser?> _signInManager;

        public ApplicationUserRepository(UserManager<ApplicationUser?> userManager, SignInManager<ApplicationUser?> signInManager)
        {
            _userManager = userManager;
           _signInManager = signInManager;
        }
        public async Task<IdentityResult> CreateUserAsync(ApplicationUser? user, string password)
        => await _userManager.CreateAsync(user, password);

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        => await _userManager.Users.SingleOrDefaultAsync(u =>
           u.Email.ToLower() == email.ToLower().Trim());

        public async Task<bool> CheckPassword(ApplicationUser? user, string password)
       => await _userManager.CheckPasswordAsync(user, password);

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        => await _userManager.FindByIdAsync(userId);


        public async Task<SignInResult> PasswordSignInAsync(ApplicationUser? user, string password)
        => await _signInManager.PasswordSignInAsync(user, password, false, false);
        

    }
}
