using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
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
        [Authorize]
        public async Task<ResponseViewModel> Logout()
        {
            ResponseViewModel response = await _authService.Logout();
            return response;
        }

    }
}
