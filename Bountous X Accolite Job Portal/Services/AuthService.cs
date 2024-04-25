using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> Register(string FirstName, string LastName, string Email, string Password)
        {


            var user = new User(FirstName, LastName, Email, Password);

            var result = await _userManager.CreateAsync(user, Password);
            if (!result.Succeeded)
            {
                user = null;
            }

            if (user == null)
            {
                return false;
            }

            // TODO: return status codes for further error handling
            return true;
        }

        public Task<bool> Login(string Email, string Password)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(string Email) 
        { 
            User checkedUser = 
        }
    }
}
