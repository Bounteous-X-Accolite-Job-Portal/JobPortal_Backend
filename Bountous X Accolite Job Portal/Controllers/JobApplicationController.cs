using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel.JobApplicationResponse;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class ApplicationController : ControllerBase
    {

        private readonly IJobApplicationService _jobApplicationService;
        private readonly IJobService _jobService;
        private readonly IDesignationService _designationService;
       
        public ApplicationController(IJobApplicationService applicationService, IJobService jobService, IDesignationService designationService)
        {
            _jobApplicationService = applicationService;
            _jobService = jobService;
           _designationService = designationService;
        }

        [HttpGet]
        [Route("jobApplication/{Id}")]
        public JobApplicationResponseViewModel GetJobApplicaionById(Guid Id)
        {
            JobApplicationResponseViewModel response = _jobApplicationService.GetJobApplicaionById(Id);
            if (response.Application == null)
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
        [Route("applicants/{JobId}")]
        public async Task<AllApplicantResponseViewModel> GetApplicantsByJobId(Guid JobId)
        {
            AllApplicantResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            if (!isEmployee || !hasPrivilege)
            {
                response = new AllApplicantResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to access applicants info.";
                return response;
            }

            response = await _jobApplicationService.GetApplicantsByJobId(JobId);
            return response;
        }

        [HttpGet]
        [Route("applicants/closedJob/{ClosedJobId}")]
        public async Task<AllApplicantResponseViewModel> GetApplicantsByClosedJobId(Guid ClosedJobId)
        {
            AllApplicantResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            if (!isEmployee || !hasPrivilege)
            {
                response = new AllApplicantResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to access applicants info.";
                return response;
            }

            response = await _jobApplicationService.GetApplicantsByClosedJobId(ClosedJobId);
            return response;
        }

        [HttpGet]
        [Route("jobApplication/candidate/{Id}")]
        public AllJobApplicationResponseViewModel GetJobApplicationByCandidateId(Guid Id)
        {
            AllJobApplicationResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            //if (!isEmployee && candidateId != Id)
            //{
            //    response = new AllJobApplicationResponseViewModel();
            //    response.Status = 401;
            //    response.Message = "You are either not loggedIn or not authorized to get applications by candidateId.";
            //    return response;
            //}

            response = _jobApplicationService.GetJobApplicationByCandidateId(Id);
            return response;
        }

        [HttpGet]
        [Route("jobApplication/isCandidateApplicable/{JobId}")]
        public ApplicationResponseViewModel IsCandidateApplicable(Guid JobId)
        {
            ApplicationResponseViewModel response = new ApplicationResponseViewModel();
            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (isEmployee)
            {
                response.Status = 400;
                response.Message = "Logged In as Employee!";
                response.name = "Employee Logged In !!";
                return response;
            }
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (candidateId == Guid.Empty)
            {
                response.Status = 401;
                response.Message = "Candidate Not Logged In !";
                response.name = "Not Logged In !!";
                return response;
            }

            Boolean result = _jobApplicationService.IsCandidateApplicable(JobId, candidateId);
            if (result)
            {
                response.Status = 200;
                response.Message = "Candidate is Applicable to apply for this job !";
                response.name = "Apply Now";
            }
            else
            {
                response.Status = 403;
                response.Message = "Candidate has Already Applyed for this job !";
                response.name = "Already Applied !";
            }

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

        [HttpGet]
        [Route("CandidateAppliedJobs/{CandidateId}")]
        public AllJobResponseViewModel GetJobsAppliedByCandidateId(Guid CandidateId)
        {
            AllJobResponseViewModel response;
            //Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            //if(candidateId!=CandidateId)
            //{
            //    response= new AllJobResponseViewModel();
            //    response.Status = 401;
            //    response.Message = "Candidate Not Found !!";
            //    return response;
            //}

            response = _jobApplicationService.GetJobsAppliedByCandidateId(CandidateId);
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
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (isEmployee || candidateId == Guid.Empty)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 403;
                response.Message = "You are not loggedIn or you are not authorised to apply this job.";
                return response;
            }

            response = await _jobApplicationService.Apply(addjobapplication, candidateId);
            return response;
        }

        [HttpPut]
        [Route("jobApplication/changeStatus/{ApplicationId}")]
        public async Task<JobApplicationResponseViewModel> ChangeJobApplicationStatus(Guid ApplicationId, ChangeStatusViewModel changeStatus)
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

            response = await _jobApplicationService.ChangeJobApplicationStatus(ApplicationId, changeStatus.statusId);
            return response;
        }
        [HttpGet]
        [Route("jobApplication/successfulApplication")]
        public IActionResult GetAllApplicationsWithSuccess()
        {

            var successApplications = _jobApplicationService.GetAllApplicationsWithSuccess();
            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            var role = User.FindFirstValue("Role");

            if (successApplications.Count == 0 && role == null || !_designationService.HasSpecialPrivilege(role))
            {
                return NotFound("No successful applications found.");
            }

            return Ok(successApplications);
        }
    
}
    }



