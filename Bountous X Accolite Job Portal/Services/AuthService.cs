using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public AuthService(SignInManager<User> signInManager, ApplicationDbContext applicationDbContext)
        {
            _signInManager = signInManager;
            _dbContext = applicationDbContext;
        }

        public async Task<ResponseViewModel> Login(LoginViewModel loginUser)
        {
            ResponseViewModel response;

            if (loginUser.Email == null || loginUser.Password == null)
            {
                response = new ResponseViewModel();
                response.Status = 404;
                response.Message = "Please fill al the details.";
                return response;
            }

            var checkUserWhetherExist = _dbContext.Users.Where(item => item.Email == loginUser.Email).ToList();
            if (checkUserWhetherExist.Count == 0)
            {
                response = new ResponseViewModel();
                response.Status = 409;
                response.Message = "This email is not registered with us. Please Register.";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, loginUser.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response = new ResponseViewModel();
                response.Status = 404;
                response.Message = "Invalid Ceredentials !";
                return response;
            }

            response = new ResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully loggedIn.";
            return response;
        }

        public async Task<ResponseViewModel> Logout()
        {
            ResponseViewModel response = new ResponseViewModel();

            await _signInManager.SignOutAsync();

            response.Status = 200;
            response.Message = "Successfully loggedOut !";
            return response;
        }
    }
}
