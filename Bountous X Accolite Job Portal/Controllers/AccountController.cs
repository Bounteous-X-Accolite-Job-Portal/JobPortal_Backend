using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;
using Bountous_X_Accolite_Job_Portal.Services;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Register(RegisterViewModel registerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill all the details.");
            }

            var isRegistered = await _authService.Register(registerUser);
            if (isRegistered)
            {
                return Ok("User successfully registered.");
            }

            return BadRequest("Unable to register user.");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Login(UserLoginViewModel loginUser)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Please fill all the details.");
            }

            bool isLoggedIn = await _authService.Login(loginUser);  
            if(isLoggedIn)
            {
                return Ok("User logged in successfully.");
            }

            return BadRequest("Please enter correct email and password");
        }
    }
}
