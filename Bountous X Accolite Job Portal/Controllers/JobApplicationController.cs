using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationController : ControllerBase
    {

        private readonly IJobApplicationService _jobApplicationService;
        private readonly IJobService _jobService;
        public ApplicationController(IJobApplicationService applicationService, IJobService jobService)
        {
            _jobApplicationService = applicationService;
            _jobService = jobService;
        }

        [HttpGet]
        [Route("jobApplication/{Id}")]
        public JobApplicationResponseViewModel GetJobApplicaionById(Guid Id)
        {
            JobApplicationResponseViewModel response = _jobApplicationService.GetJobApplicaionById(Id);
            if(response.Application == null)
            {
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee && candidateId != response.Application.CandidateId)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to access application.";
                return response;
            }

            return response;
        }

        [HttpGet]
        [Route("jobApplication/candidate/{Id}")]
        public AllJobApplicationResponseViewModel GetJobApplicationByCandidateId(Guid Id)
        {
            AllJobApplicationResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee && candidateId != Id)
            {
                response = new AllJobApplicationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to get applications by candidateId.";
                return response;
            }

            response = _jobApplicationService.GetJobApplicationByCandidateId(Id);
            return response;
        }

        [HttpGet]
        [Route("jobApplication/job/{Id}")]
        public AllJobApplicationResponseViewModel GetJobApplicationByJobId(Guid Id)
        {
            AllJobApplicationResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
            {
                response = new AllJobApplicationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to get application by jobId.";
                return response;
            }

            return _jobApplicationService.GetJobApplicationByJobId(Id);
        }

        [HttpGet]
        [Route("jobApplication/closedJob/{Id}")]
        public AllJobApplicationResponseViewModel GetJobApplicationByClosedJobId(Guid Id)
        {
            AllJobApplicationResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
            {
                response = new AllJobApplicationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to get application by jobId.";
                return response;
            }

            return _jobApplicationService.GetJobApplicationByClosedJobId(Id);
        }

        [HttpGet]
        [Route("closedJobApplication/candidate/{Id}")]
        public AllJobApplicationResponseViewModel GetClosedJobApplicationByCandidateId(Guid Id)
        {
            AllJobApplicationResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee && candidateId != Id)
            {
                response = new AllJobApplicationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to get applications by candidateId.";
                return response;
            }

            response = _jobApplicationService.GetClosedJobApplicationByCandidateId(Id);
            return response;
        }

        [HttpPost]
        [Route("apply")]
        public async Task<JobApplicationResponseViewModel> Apply(AddJobApplication addjobapplication)
        {
            JobApplicationResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 400;
                response.Message = "PLease fill all the details.";
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (isEmployee || candidateId == Guid.Empty)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 403;
                response.Message = "You are not loggedIn or you are not authorised to apply this job.";
            }
        
            response = await _jobApplicationService.Apply(addjobapplication, candidateId);
            return response;
        }

        [HttpPut]
        [Route("jobApplication/changeStatus/{ApplicationId}/{StatusId}")]
        public async Task<JobApplicationResponseViewModel> ChangeJobApplicationStatus(Guid ApplicationId, int StatusId)
        {
            JobApplicationResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to change status of job application.";
                return response;
            }

            JobApplicationResponseViewModel applicationResponse = GetJobApplicaionById(ApplicationId);
            if(applicationResponse.Application == null)
            {
                return applicationResponse;
            }

            JobResponseViewModel jobResponse = _jobService.GetJobById((Guid)applicationResponse.Application.JobId);
            if(jobResponse.job == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = jobResponse.Status;
                response.Message = jobResponse.Message;
                return response;
            }

            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if(employeeId != jobResponse.job.EmployeeId)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 401;
                response.Message = "You are not authorised to change status of this job.";
                return response;
            }

            response = await _jobApplicationService.ChangeJobApplicationStatus(ApplicationId, StatusId);
            if(response.Application == null)
            {
                return response;
            }

            response = new JobApplicationResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully changed the status of given job.";
            return response;
        }
    }
}
