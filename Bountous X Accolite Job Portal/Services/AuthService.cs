using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.JwtFeatures;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtHandler _jwtHandler;
        private readonly IDistributedCache _cache;
        private readonly IEmployeeAccountService _employeeAccountService;
        private readonly ICandidateAccountService _candidateAccountServices;
        public AuthService(SignInManager<User> signInManager, ApplicationDbContext applicationDbContext, JwtHandler jwtHandler, IDistributedCache cache, IEmployeeAccountService employeeAccountService, ICandidateAccountService candidateAccountServices)
        {
            _signInManager = signInManager;
            _dbContext = applicationDbContext;
            _jwtHandler = jwtHandler;
            _cache = cache;
            _employeeAccountService = employeeAccountService;
            _candidateAccountServices = candidateAccountServices;
        }

        public async Task<LoginServiceResponseViewModel> Login(LoginViewModel loginUser)
        {
            LoginServiceResponseViewModel response;

            string key = $"getUserByEmail-{loginUser.Email}";
            string? getUserByEmailFromCache = await _cache.GetStringAsync(key);

            User checkUserWhetherExist;
            if (string.IsNullOrEmpty(getUserByEmailFromCache))
            {
                checkUserWhetherExist = _dbContext.Users.Where(item => item.Email == loginUser.Email).FirstOrDefault();
                if (checkUserWhetherExist == null)
                {
                    response = new LoginServiceResponseViewModel();
                    response.Status = 409;
                    response.Message = "This email is not registered with us. Please Register.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(checkUserWhetherExist));
            }
            else
            {
                checkUserWhetherExist = JsonSerializer.Deserialize<User>(getUserByEmailFromCache);
            }

            EmployeeResponseViewModel loginEmployee = null;
            if (checkUserWhetherExist.EmpId != null)
            {
                loginEmployee = await _employeeAccountService.GetEmployeeById((Guid)checkUserWhetherExist.EmpId);
                if (loginEmployee.Employee != null && loginEmployee.Employee.Inactive)
                {
                    response = new LoginServiceResponseViewModel();
                    response.Status = 401;
                    response.Message = "Your account has been disabled, please contact administrator.";
                    return response;
                }
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

            var user = checkUserWhetherExist;

            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = await _jwtHandler.GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            if (checkUserWhetherExist.EmpId == null)
            {
                var candidate = await _candidateAccountServices.GetCandidateById((Guid)user.CandidateId);
                response.Candidate = candidate.Candidate;
            }
            else
            {
                response.Employee = loginEmployee.Employee;
            }

            response.Token = token;
            response.User = checkUserWhetherExist;
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
