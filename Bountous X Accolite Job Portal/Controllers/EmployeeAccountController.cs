using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAccountController : ControllerBase
    {
        private readonly IEmployeeAuthService _employeeAuthService;
        public EmployeeAccountController(IEmployeeAuthService employeeAuthService)
        {
            _employeeAuthService = employeeAuthService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(EmployeeRegisterViewModel employee)
        {
            if (employee == null|| employee.Email == null || employee.Password == null || employee.FirstName == null || employee.LastName == null || employee.EmpId == null)
            {
                return BadRequest("Please Enter all feilds to Login.");
            }

            var isRegistered = await _employeeAuthService.Register(employee);
            if (isRegistered)
            {
                return Ok("Employee successfully logged in.");
            }

            return BadRequest("Invalid email address or password.");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(EmployeeLoginViewModel employee)
        {
            if(employee == null || employee.Email == null || employee.Password == null)
            {
                return BadRequest("Please Enter all feilds to Login.");
            }

            var isLoggedIn = await _employeeAuthService.Login(employee);
            if(isLoggedIn)
            {
                return Ok("Employee successfully logged in.");
            }

            return BadRequest("Invalid email address or password.");
        }
    }
}
