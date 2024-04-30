using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Http;
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
        public AccountController(IAuthService authService, UserManager<User> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ResponseViewModel> Login(LoginViewModel loginUser)
        {
            ResponseViewModel response;

            if(!ModelState.IsValid)
            {
                response = new ResponseViewModel();
                response.Status = 404;
                response.Message = "Please fill all the details.";
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            if(user != null)
            {
                response = new ResponseViewModel();
                response.Status = 403;
                response.Message = "Please logout to login again.";
                return response;
            }

            response = await _authService.Login(loginUser);
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
