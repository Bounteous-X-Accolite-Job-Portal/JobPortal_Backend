using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;
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
    public class JobController : ControllerBase
    {
        private readonly IJobService _job;
        private readonly UserManager<User> _userManager;

        public JobController(IJobService job, UserManager<User> userManager)
        {
            _job = job;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("getAllJobs")]
        public AllJobResponseViewModel GetAllJobs()
        {
            return _job.GetAllJobs();
        }

        [HttpGet]
        [Route("getJob/{Id}")]
        public JobResponseViewModel GetJobById(Guid id)
        {
            return _job.GetJobById(id);
        }

        [HttpPost]
        [Route("AddJob")]
        public async Task<JobResponseViewModel> AddJob(CreateJobViewModel job)
        {
            JobResponseViewModel response = new JobResponseViewModel();
            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            var emp = await _userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Add Job";
                return response;
            }

            response = await _job.AddJob(job,(Guid)emp.EmpId);
            return response;
        }

        [HttpPut]
        [Route("UpdateJob")]
        public async Task<JobResponseViewModel> EditJob(EditJobViewModel job)
        {
            JobResponseViewModel response = new JobResponseViewModel();
            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            var emp = await _userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Edit Job";
                return response;
            }

            response = await _job.EditJob(job);
            return response;
        }

        [HttpDelete]
        [Route("DeleteJob/{Id}")]
        public async Task<JobResponseViewModel> DeleteJob(Guid JobId)
        {
            JobResponseViewModel response = new JobResponseViewModel();
            var emp = await _userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Delete Job";
                return response;
            }

            response = await _job.DeleteJob(JobId);
            return response;
        }
    }
}
