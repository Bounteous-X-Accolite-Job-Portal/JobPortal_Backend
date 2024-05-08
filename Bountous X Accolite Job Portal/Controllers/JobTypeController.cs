using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.JobTypeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobTypeViewModel.JobTypeResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTypeController : ControllerBase
    {
        private readonly IJobTypeService _jobTypeService;

        public JobTypeController(IJobTypeService jobTypeService)
        {
            _jobTypeService = jobTypeService;
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
        [Authorize]
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("EmployeeId"));
            if (!isEmployee || employeeId == Guid.Empty)
            {
                response = new JobTypeResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Add Job Type";
                return response;
            }

            response = await _jobTypeService.AddJobType(jobType, employeeId);
            return response;
        }

        [HttpDelete]
        [Route("DeleteJobType")]
        [Authorize]
        public async Task<JobTypeResponseViewModel> DeleteJobType(Guid Id)
        {
            JobTypeResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
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
        [Authorize]
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
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
