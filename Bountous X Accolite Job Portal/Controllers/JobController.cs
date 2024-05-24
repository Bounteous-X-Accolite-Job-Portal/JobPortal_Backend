using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.ClosedJobViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _job;
        public JobController(IJobService job)
        {
            _job = job;
        }

        [HttpGet]
        [Route("getJob/{Id}")]
        public async Task<JobResponseViewModel> GetJobById(Guid Id)
        {
            return await _job.GetJobById(Id);
        }

        [HttpGet]
        [Route("getClosedJob/{Id}")]
        public async Task<ClosedJobResponseViewModel> GetClosedJobById(Guid Id)
        {
            return await _job.GetClosedJobById(Id);
        }

        [HttpGet]
        [Route("getAllJobs")]
        public async Task<AllJobResponseViewModel> GetAllJobs()
        {
            return await _job.GetAllJobs();
        }

        [HttpGet]
        [Route("getAllClosedJobs")]
        public async Task<AllClosedJobResponseViewModel> GetAllClosedJobs()
        {
            return await _job.GetAllClosedJobs(); 
        }

        [HttpGet]
        [Route("getAllJobsByEmployee/{Id}")]
        [Authorize]
        public async Task<AllJobResponseViewModel> GetAllJobsByEmployeeId(Guid Id)
        {
            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
            {
                AllJobResponseViewModel response = new AllJobResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Get Jobs By Employee Id !";
                return response;
            }

            return await _job.GetAllJobsByEmployeeId(Id);
        }

        [HttpGet]
        [Route("getAllClosedJobsByEmployee/{Id}")]
        [Authorize]
        public async Task<AllClosedJobResponseViewModel> GetAllClosedJobsByEmployeeId(Guid Id)
        {
            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
            {
                AllClosedJobResponseViewModel response = new AllClosedJobResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Get Jobs By Employee Id !";
                return response;
            }

            return await _job.GetAllClosedJobsByEmployeeId(Id);
        }

        [HttpPost]
        [Route("AddJob")]
        [Authorize]
        public async Task<JobResponseViewModel> AddJob(CreateJobViewModel job)
        {
            JobResponseViewModel response = new JobResponseViewModel();
            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee || !hasPrivilege || employeeId == Guid.Empty)
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Add Job";
                return response;
            }

            response = await _job.AddJob(job, employeeId);
            return response;
        }

        [HttpPut]
        [Route("UpdateJob")]
        [Authorize]
        public async Task<JobResponseViewModel> EditJob(EditJobViewModel job)
        {
            JobResponseViewModel response = new JobResponseViewModel();
            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee || employeeId == Guid.Empty)
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Edit Job";
                return response;
            }

            var checkJob = (await GetJobById(job.JobId)).job;
            if(checkJob == null || checkJob.EmployeeId != employeeId)
            {
                response.Status = 403;
                response.Message = "You are not authorised to edit job.";
                return response;
            }

            response = await _job.EditJob(job);
            return response;
        }

        [HttpDelete]
        [Route("DeleteJob/{Id}")]
        [Authorize]
        public async Task<JobResponseViewModel> DeleteJob(Guid JobId)
        {
            JobResponseViewModel response = new JobResponseViewModel();

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee || employeeId == Guid.Empty)
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Delete Job";
                return response;
            }

            var checkJob = (await GetJobById(JobId)).job;
            if (checkJob == null || checkJob.EmployeeId != employeeId)
            {
                response.Status = 403;
                response.Message = "You are not authorised to delete this job.";
                return response;
            }

            response = await _job.DeleteJob(JobId);
            return response;
        }
    }
}
