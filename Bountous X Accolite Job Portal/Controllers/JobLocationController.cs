
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel.JobLocationResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobLocationController : ControllerBase
    {
        private readonly IJobLocationService _jobLocationService;
        private readonly UserManager<User> _userManager;

        public JobLocationController(IJobLocationService jobLocationService, UserManager<User> userManager)
        {
            _jobLocationService = jobLocationService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("getAllJobLocations")]
        public AllJobLocationResponseViewModel getAllJobLocations()
        {
            return _jobLocationService.GetAllJobLocations();
        }

        [HttpGet]
        [Route("getJobLocation/{Id}")]
        public JobLocationResponseViewModel getJobLocation(Guid Id)
        {
            return _jobLocationService.GetLocationById(Id);
        }

        [HttpPost]
        [Route("AddJobLocation")]
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
            var emp = await _userManager.GetUserAsync(User);
            if(emp==null || emp.EmpId==null)
            {
                response = new JobLocationResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Add Location";
                return response;
            }

            response = await _jobLocationService.AddLocation(location,(Guid)emp.EmpId);
            return response;
        }

        [HttpDelete]
        [Route("DeleteLocation/{Id}")]
        public async Task<JobLocationResponseViewModel> DeleteLocation(Guid locationId)
        {
            JobLocationResponseViewModel response;
           
            var emp = await _userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
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
            var emp = await _userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
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
