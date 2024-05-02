using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel;
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

        public async Task<LoginServiceResponseViewModel> Login(LoginViewModel loginUser)
        {
            LoginServiceResponseViewModel response;

            if (loginUser.Email == null || loginUser.Password == null)
            {
                response = new LoginServiceResponseViewModel();
                response.Status = 404;
                response.Message = "Please fill al the details.";
                return response;
            }

            var checkUserWhetherExist = _dbContext.Users.Where(item => item.Email == loginUser.Email).ToList();
            if (checkUserWhetherExist.Count == 0)
            {
                response = new LoginServiceResponseViewModel();
                response.Status = 409;
                response.Message = "This email is not registered with us. Please Register.";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, loginUser.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response = new LoginServiceResponseViewModel();
                response.Status = 404;
                response.Message = "Invalid Ceredentials !";
                return response;
            }

            response = new LoginServiceResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully loggedIn.";

            if (checkUserWhetherExist[0].EmpId == null)
            {
                var candidate = _dbContext.Candidates.Find(checkUserWhetherExist[0].CandidateId);
                response.Candidate = new CandidateViewModel(candidate);
            }
            else
            {
                var employee = _dbContext.Employees.Find(checkUserWhetherExist[0].EmpId);
                response.Employee = new EmployeeViewModels(employee);
            }
            response.User = checkUserWhetherExist[0];
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
