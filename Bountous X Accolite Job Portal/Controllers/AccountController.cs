using Bountous_X_Accolite_Job_Portal.JwtFeatures;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly JwtHandler _jwtHandler;
        public AccountController(IAuthService authService, UserManager<User> userManager, JwtHandler jwtHandler)
        {
            _authService = authService;
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        [HttpPost]
        [Route("login")]
        public async Task<LoginResponseViewModel> Login(LoginViewModel loginUser)
        {
            LoginResponseViewModel response;

            if(!ModelState.IsValid)
            {
                response = new LoginResponseViewModel();
                response.Status = 404;
                response.Message = "Please fill all the details.";
                return response;
            }

            LoginServiceResponseViewModel res = await _authService.Login(loginUser);
            if(res.User == null)
            {
                response = new LoginResponseViewModel();
                response.Status = res.Status;
                response.Message = res.Message;
                return response;
            }

            response = new LoginResponseViewModel();
            response.Status = res.Status;
            response.Message = res.Message;
            response.Candidate = res.Candidate;
            response.Employee = res.Employee;
            response.Token = res.Token;
            return response;
        }

        [HttpPost]
        [Route("logout")]
        public async Task<ResponseViewModel> Logout()
        {
            ResponseViewModel response = await _authService.Logout();
            return response;
        }

    }
}
