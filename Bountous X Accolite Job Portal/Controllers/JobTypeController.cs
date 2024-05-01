using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel.JobLocationResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobTypeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobTypeViewModel.JobTypeResponseViewModel;
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
    public class JobTypeController : ControllerBase
    {
        private readonly IJobTypeService _jobTypeService;
        private readonly UserManager<User> _userManager;

        public JobTypeController(IJobTypeService jobTypeService, UserManager<User> userManager)
        {
            _jobTypeService = jobTypeService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("getAllJobTypes")]
        public AllJobTypeResponseViewModel GetAllJobTypes()
        {
            return _jobTypeService.GetAllJobTypes();
        }
        
        [HttpGet]
        [Route("getJobType/{Id}")]
        public JobTypeResponseViewModel GetJobTypeById(Guid Id)
        {
            return _jobTypeService.GetJobTypeById(Id);
        }

        [HttpPost]
        [Route("AddJobType")]
        public async Task<JobTypeResponseViewModel> AddJobType(CreateJobTypeViewModel jobType)
        {
            JobTypeResponseViewModel response;
            if (!ModelState.IsValid)
            {
                response = new JobTypeResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }
            var emp = await _userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
            {
                response = new JobTypeResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Add Job Type";
                return response;
            }

            response = await _jobTypeService.AddJobType(jobType,(Guid)emp.EmpId);
            return response;
        }

        [HttpDelete]
        [Route("DeleteJobType")]
        public async Task<JobTypeResponseViewModel> DeleteJobType(Guid Id)
        {
            JobTypeResponseViewModel response;
            var emp = await _userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
            {
                response = new JobTypeResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Delete Job Type";
                return response;
            }

            response = await _jobTypeService.DeleteJobType(Id);
            return response;
        }

        [HttpPut]
        [Route("UpdateJobType")]
        public async Task<JobTypeResponseViewModel> UpdateJobType(EditJobTypeViewModel jobType)
        {
            JobTypeResponseViewModel response;
            if (!ModelState.IsValid)
            {
                response = new JobTypeResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }
            var emp = await _userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
            {
                response = new JobTypeResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Edit Job Type";
                return response;
            }

            response = await _jobTypeService.UpdateJobType(jobType);
            return response;
        }
    }
}
