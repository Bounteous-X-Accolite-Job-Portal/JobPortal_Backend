
using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel.JobLocationResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobLocationController : ControllerBase
    {
        private readonly IJobLocationService _jobLocationService;
        private readonly IDesignationService _designationService;
        public JobLocationController(IJobLocationService jobLocationService, IDesignationService designationService)
        {
            _jobLocationService = jobLocationService;
            _designationService = designationService;
        }

        [HttpGet]
        [Route("getAllJobLocations")]
        public AllJobLocationResponseViewModel getAllJobLocations()
        {
            return _jobLocationService.GetAllJobLocations();
        }

        [HttpGet]
        [Route("getJobLocation/{Id}")]
        [Authorize]
        public JobLocationResponseViewModel getJobLocation(Guid Id)
        {
            return _jobLocationService.GetLocationById(Id);
        }

        [HttpPost]
        [Route("AddJobLocation")]
        [Authorize]
        public async Task<JobLocationResponseViewModel> AddJobLocation(CreateJobLocationViewModel location)
        {
            JobLocationResponseViewModel response;
            if(!ModelState.IsValid)
            {
                response = new JobLocationResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role) || employeeId == Guid.Empty)
            {
                response = new JobLocationResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Add Location";
                return response;
            }

            response = await _jobLocationService.AddLocation(location, employeeId);
            return response;
        }

        [HttpDelete]
        [Route("DeleteLocation/{Id}")]
        [Authorize]
        public async Task<JobLocationResponseViewModel> DeleteLocation(Guid locationId)
        {
            JobLocationResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role))
            {
                response = new JobLocationResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Delete Location";
                return response;
            }

            response = await _jobLocationService.DeleteLocation(locationId);
            return response;
        }

        [HttpPut]
        [Route("UpdateJobLocation")]
        [Authorize]
        public async Task<JobLocationResponseViewModel> UpdateLocation(EditJobLocationViewModel location)
        {
            JobLocationResponseViewModel response;
            if (!ModelState.IsValid)
            {
                response = new JobLocationResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role))
            {
                response = new JobLocationResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Edit Location";
                return response;
            }

            response = await _jobLocationService.UpdateLocation(location);
            return response;
        }
    }
}
