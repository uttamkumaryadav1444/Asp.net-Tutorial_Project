using EventManagementWebApp.Models;
using EventManagementWebApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EventManagementWebApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User with this email already exists." });
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
            }

            return result;
        }

        public async Task<SignInResult> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    var existingClaim = (await _userManager.GetClaimsAsync(user))
                        .FirstOrDefault(c => c.Type == "OrganizerId");

                    if (existingClaim == null)
                    {
                        await _userManager.AddClaimAsync(user, new Claim("OrganizerId", user.Id.ToString()));
                    }

                    return result;
                }
            }

            return SignInResult.Failed;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> AddOrganizer(string email, string password)
        {
            try
            {
                var user = new User
                {
                    UserName = email,
                    Email = email
                };

                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    Console.WriteLine("Error Creating User: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    return false;
                }

                await _userManager.AddToRoleAsync(user, "Organizer");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

    }
}
