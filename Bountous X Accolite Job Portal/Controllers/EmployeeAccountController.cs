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
    [Authorize]
    public class EmployeeAccountController : ControllerBase
    {
        private readonly IEmployeeAccountService _employeeAuthService;
        private readonly IDesignationService _designationService;
        private readonly I_InterviewService _interviewService;
        private readonly IReferralService _referralService;
        private readonly IJobService _jobService;
        public EmployeeAccountController(IEmployeeAccountService employeeAuthService, IDesignationService designationService, I_InterviewService interviewService, IReferralService referralService, IJobService jobService)
        {
            _employeeAuthService = employeeAuthService;
            _designationService = designationService;   
            _interviewService = interviewService;
            _jobService = jobService;
            _referralService = referralService;
        }

        [HttpGet]
        [Route("getAllEmployees")]
        public async Task<AllEmployeesResponseViewModel> GetAllEmployees()
        {
            AllEmployeesResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            if (!isEmployee || !hasPrivilege)
            {
                response = new AllEmployeesResponseViewModel();
                response.Status = 401;
                response.Message = "You are not loggedIn or not authorised to do get all employees.";
                return response;
            }

            response = await _employeeAuthService.GetAllEmployees();
            return response;
        }

        [HttpGet]
        [Route("employee/{Id}")]
        public async Task<EmployeeResponseViewModel> GetEmployeesById(Guid Id)
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

            response = await _employeeAuthService.GetEmployeeById(Id);
            return response;
        }

        [HttpPost]
        [Route("register")]
        public async Task<EmployeeResponseViewModel> Register(EmployeeRegisterViewModel employee)
        {
            EmployeeResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new EmployeeResponseViewModel();
                response.Status = 404;
                response.Message = "Please fill all the details.";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || !hasPrivilege)
            {
                response = new EmployeeResponseViewModel();
                response.Status = 401;
                response.Message = "You are not loggedIn or not authorised to do add other employees.";
                return response;
            }

            DesignationResponseViewModel designation = await _designationService.GetDesignationById(employee.DesignationId);
            if (designation.Designation == null)
            {
                response = new EmployeeResponseViewModel();
                response.Status = designation.Status;
                response.Message = designation.Message;
                return response;
            }

            if (string.Equals(designation.Designation.DesignationName.ToLower(), "admin") && !_designationService.HasSpecialPrivilege(role))
            {
                response = new EmployeeResponseViewModel();
                response.Status = 401;
                response.Message = "You are not loggedIn or not authorised to do add other employees with this designation.";
                return response;
            }

            response = await _employeeAuthService.Register(employee);
            return response;
        }

        [HttpPut]
        [Route("disableAccount/{Id}")]
        [Route("enableAccount/{Id}")]
        public async Task<EmployeeResponseViewModel> ToggleEmployeeAccountStatus(Guid Id)
        {
            EmployeeResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || !hasPrivilege)
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

            response = await _employeeAuthService.ToggleEmployeeAccountStatus(Id, _designationService.HasSpecialPrivilege(role));
            return response;
        }

        [HttpGet]
        [Route("profileData/{Id}")]
        public async Task<ProfileDataResponseViewModel> GetProfileData(Guid Id)
        {
            ProfileDataResponseViewModel response = new ProfileDataResponseViewModel();

            var employee = await _employeeAuthService.GetEmployeeById(Id);
            if(employee.Employee == null)
            {
                response.Status = 404;
                response.Message = "Employee with this Id does not exist.";
                return response;
            }

            var interview = await _interviewService.GetAllInterviewsForInterviewer(Id);
            var referral = await _referralService.GetAllReferralsOfLoggedInEmployee(Id);
            var jobs = await _jobService.GetAllJobsByEmployeeId(Id);
            var closedJobs = await _jobService.GetAllClosedJobsByEmployeeId(Id);

            response.Status = 200;
            response.Message = "Successfully retrieved employee profile.";
            response.InterviewTaken = interview.allInterviews.Count;
            response.CandidatesReferred = referral.Referrals.Count;
            response.JobAdded = jobs.allJobs.Count + closedJobs.ClosedJobs.Count;
            return response;
        }
    }
}
