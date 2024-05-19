﻿using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel.InterviewResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InterviewController : ControllerBase
    {
        private readonly I_InterviewService _InterviewService;
        private readonly IDesignationService _designationService;
        public InterviewController(I_InterviewService interviewService, IDesignationService designationService)
        {
            _InterviewService = interviewService;
            _designationService = designationService;
        }

        [HttpGet]
        [Route("GetAllInterviewsForInterviewer")]
        [Authorize]
        public async Task<All_InterviewResponseViewModel> GetAllInterviewsForInterviewer()
        {
            All_InterviewResponseViewModel response = new All_InterviewResponseViewModel();

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to See Interviewer Interviews !";
            }
            else
            {
                response = _InterviewService.GetAllInterviewsForInterviewer(employeeId);
            }
            return response;
        }

        [HttpGet]
        [Route("getAllInterviewsByApplicationId/{ApplicationId}")]
        [Authorize]
        public async Task<All_InterviewResponseViewModel> GetAllInterviewsByApplicationId(Guid ApplicationId)
        {
            All_InterviewResponseViewModel response = new All_InterviewResponseViewModel();

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            if (!isEmployee || !hasPrivilege)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to get all Interviewer by applicationId !";
            }
            else
            {
                response = _InterviewService.GetAllInterviewByApplicationId(ApplicationId);
            }
            return response;
        }

        [HttpGet]
        [Route("getAllApplicantInterviewsByApplicationId/{ApplicationId}")]
        [Authorize]
        public async Task<AllApplicantInterviewResponseViewModel> GetAllApplicantInterviewsByApplicationId(Guid ApplicationId)
        {
            AllApplicantInterviewResponseViewModel response = new AllApplicantInterviewResponseViewModel();

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            if (!isEmployee || !hasPrivilege)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to get all Interviewer by applicationId !";
            }
            else
            {
                response = await _InterviewService.GetAllApplicantInterviewByApplicantionId(ApplicationId);
            }
            return response;
        }

        [HttpGet]
        [Route("getInterviewById/{Id}")]
        public InterviewResponseViewModel GetInterviewById(Guid Id)
        {
            return _InterviewService.GetInterviewById(Id);
        }

        [HttpPost]
        [Route("AddInterview")]
        public async Task<InterviewResponseViewModel> AddInterview(CreateInterviewViewModel interview)
        {
            InterviewResponseViewModel response;
            if (!ModelState.IsValid)
            {
                response = new InterviewResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role))
            {
                response = new InterviewResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Add Interview";
                return response;
            }

            response = await _InterviewService.AddInterview(interview, employeeId);
            return response;
        }

        [HttpDelete]
        [Route("DeleteInterview")]
        public async Task<InterviewResponseViewModel> DeleteInterview(Guid Id)
        {
            InterviewResponseViewModel response;

            InterviewResponseViewModel res = GetInterviewById(Id);
            if(res.Interview == null)
            {
                response = new InterviewResponseViewModel();
                response.Status = res.Status;
                response.Message = "Interview with this Id does not exist.";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee || employeeId != res.Interview.EmpId)
            {
                response = new InterviewResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Delete Interview";
                return response;
            }

            response = await _InterviewService.DeleteInterview(Id);
            return response;
        }

        [HttpPut]
        [Route("UpdateInterview")]
        public async Task<InterviewResponseViewModel> EditInterview(EditInterviewViewModel interview)
        {
            InterviewResponseViewModel response;
            if (!ModelState.IsValid)
            {
                response = new InterviewResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            InterviewResponseViewModel res = GetInterviewById(interview.InterviewId);
            if (res.Interview == null)
            {
                response = new InterviewResponseViewModel();
                response.Status = res.Status;
                response.Message = "Interview with this Id does not exist.";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee || employeeId != res.Interview.EmpId)
            {
                response = new InterviewResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Edit Interview";
                return response;
            }

            response = await _InterviewService.EditInterview(interview);
            return response;
        }
    }
}
