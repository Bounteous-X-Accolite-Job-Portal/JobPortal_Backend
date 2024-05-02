using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAccountController : ControllerBase
    {
        private readonly IEmployeeAccountService _employeeAuthService;
        public EmployeeAccountController(IEmployeeAccountService employeeAuthService)
        {
            _employeeAuthService = employeeAuthService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<EmployeeResponseViewModel> Register(EmployeeRegisterViewModel employee)
        {
            EmployeeResponseViewModel response;

            if(!ModelState.IsValid)
            {
                response = new EmployeeResponseViewModel();
                response.Status = 404;
                response.Message = "Please fill all the details.";
                return response;
            }

            var email = User.FindFirstValue(ClaimTypes.Name);
            if (email != null)
            {
                response = new EmployeeResponseViewModel();
                response.Status = 403;
                response.Message = "Please first logout to login.";
                return response;
            }

            response = await _employeeAuthService.Register(employee);
            return response;
        }

    }
}
