using Microsoft.AspNetCore.Identity;

namespace StudentsApi.Services
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthenticateService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<bool> RegisterUserAsync(string email, string password)
        {
            var appUser = new IdentityUser
            {
                UserName = email,
                Email = email,
            };

            var result = await _userManager.CreateAsync(appUser, password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(appUser, isPersistent: false);
            }
            return result.Succeeded;
        }

        public async Task<bool> AuthenticateUserAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}