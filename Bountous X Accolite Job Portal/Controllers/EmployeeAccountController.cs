using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAccountController : ControllerBase
    {
        private readonly IEmployeeAccountService _employeeAuthService;
        private readonly IDesignationService _designationService;
        public EmployeeAccountController(IEmployeeAccountService employeeAuthService, IDesignationService designationService)
        {
            _employeeAuthService = employeeAuthService;
            _designationService = designationService;   
        }

        [HttpGet]
        [Route("getAllEmployees")]
        [Authorize]
        public AllEmployeesResponseViewModel GetAllEmployees()
        {
            AllEmployeesResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role))
            {
                response = new AllEmployeesResponseViewModel();
                response.Status = 401;
                response.Message = "You are not loggedIn or not authorised to do get all employees.";
                return response;
            }

            response = _employeeAuthService.GetAllEmployees();
            return response;
        }

        [HttpGet]
        [Route("employee/{Id}")]
        [Authorize]
        public EmployeeResponseViewModel GetEmployeesById(Guid Id)
        {
            EmployeeResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
            {
                response = new EmployeeResponseViewModel();
                response.Status = 401;
                response.Message = "You are not loggedIn or not authorised to do get info about employees.";
                return response;
            }

            response = _employeeAuthService.GetEmployeeById(Id);
            return response;
        }

        [HttpPost]
        [Route("register")]
        //[Authorize]
        public async Task<EmployeeResponseViewModel> Register(EmployeeRegisterViewModel employee)
        {
            EmployeeResponseViewModel response;

            //if(!ModelState.IsValid)
            //{
            //    response = new EmployeeResponseViewModel();
            //    response.Status = 404;
            //    response.Message = "Please fill all the details.";
            //    return response;
            //}

            //var email = User.FindFirstValue("Email");
            //if (email == null)
            //{
            //    response = new EmployeeResponseViewModel();
            //    response.Status = 401;
            //    response.Message = "You are not loggedIn or not authorised to do add other employees.";
            //    return response;
            //}

            //bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            //var role = User.FindFirstValue("Role");
            //if (!isEmployee || role == null || !_designationService.HasPrivilege(role))
            //{
            //    response = new EmployeeResponseViewModel();
            //    response.Status = 401;
            //    response.Message = "You are not loggedIn or not authorised to do add other employees.";
            //    return response;
            //}

            //DesignationResponseViewModel designation = await _designationService.GetDesignationById(employee.DesignationId);
            //if(designation.Designation == null)
            //{
            //    response = new EmployeeResponseViewModel();
            //    response.Status = designation.Status;
            //    response.Message = designation.Message;
            //    return response;
            //}
             
            //if(String.Equals(designation.Designation.DesignationName.ToLower(), "admin") && !_designationService.HasSpecialPrivilege(role))
            //{
            //    response = new EmployeeResponseViewModel();
            //    response.Status = 401;
            //    response.Message = "You are not loggedIn or not authorised to do add other employees with this designation.";
            //    return response;
            //}

            response = await _employeeAuthService.Register(employee);
            return response;
        }

        [HttpPut]
        [Route("disableAccount/{Id}")]
        [Authorize]
        public async Task<EmployeeResponseViewModel> DisableEmployeeAccount(Guid Id)
        {
            EmployeeResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role))
            {
                response = new EmployeeResponseViewModel();
                response.Status = 401;
                response.Message = "You are not loggedIn or not authorised to disable other employees.";
                return response;
            }

            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if(employeeId == Id)
            {
                response = new EmployeeResponseViewModel();
                response.Status = 401;
                response.Message = "You cannot diable your own account.";
                return response;
            }

            response = await _employeeAuthService.DisableEmployeeAccount(Id, _designationService.HasSpecialPrivilege(role));
            return response;
        }
    }
}
