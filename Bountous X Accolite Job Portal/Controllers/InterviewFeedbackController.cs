using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackViewModel.InterviewFeedbackResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InterviewFeedbackController : ControllerBase
    {
        private readonly I_InterviewFeedbackService _InterviewFeedbackService;

        public InterviewFeedbackController(I_InterviewFeedbackService interviewFeedbackService)
        {
            _InterviewFeedbackService = interviewFeedbackService;
        }

        [HttpGet]
        [Route("getAllInterviewFeedbacks/{EmployeeId}")]
        public async Task<AllInterviewFeedbackResponseViewModel> GetAllInterviewFeedbacksByAEmployee(Guid EmployeeId)
        {
            AllInterviewFeedbackResponseViewModel response = new AllInterviewFeedbackResponseViewModel();

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("EmployeeId"));
            if (!isEmployee || employeeId != EmployeeId)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to See All Interviews Feedbacks";
            }
            else
            {
                response = _InterviewFeedbackService.GetAllInterviewFeedbacksByAEmployee(EmployeeId);
            }

            return response;
        }

        [HttpGet]
        [Route("GetInterviewFeedbackById/{Id}")]
        public async Task<InterviewFeedbackResponseViewModel> GetInterviewFeedbackById(Guid Id)
        {
            InterviewFeedbackResponseViewModel response = new InterviewFeedbackResponseViewModel();

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (!isEmployee)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to See Interview Feedback";
            }
            else
            {
                response = _InterviewFeedbackService.GetInterviewFeedbackById(Id);
            }
            return response;
        }

        [HttpPost]
        [Route("AddInterviewFeedback")]
        public async Task<InterviewFeedbackResponseViewModel> AddInterviewFeedback(CreateInterviewFeedbackViewModel interviewFeedback)
        {
            InterviewFeedbackResponseViewModel response = new InterviewFeedbackResponseViewModel();
            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("EmployeeId"));
            if (!isEmployee || employeeId == Guid.Empty)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to Add Interview Feedback";
                return response;
            }

            response = await _InterviewFeedbackService.AddInterviewFeedback(interviewFeedback, employeeId);
            return response;
        }

        [HttpDelete]
        [Route("DeleteInterviewFeedback")]
        public async Task<InterviewFeedbackResponseViewModel> DeleteInterviewFeedback(Guid Id)
        {
            InterviewFeedbackResponseViewModel response = new InterviewFeedbackResponseViewModel();

            InterviewFeedbackResponseViewModel feedback = await GetInterviewFeedbackById(Id);
            if(feedback.interviewFeedback == null)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to Delete Interview Feedback";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("EmployeeId"));
            if (!isEmployee || employeeId != feedback.interviewFeedback.EmployeeId)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to Delete Interview Feedback";
                return response;
            }

            response = await _InterviewFeedbackService.DeleteInterviewFeedback(Id);

            return response;
        }

        [HttpPut]
        [Route("UpdateInterviewFeedback")]
        public async Task<InterviewFeedbackResponseViewModel> EditInterviewFeedback(EditInterviewFeedbackViewModel interviewFeedback)
        {
            InterviewFeedbackResponseViewModel response = new InterviewFeedbackResponseViewModel();

            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            InterviewFeedbackResponseViewModel feedback = await GetInterviewFeedbackById(interviewFeedback.FeedbackId);
            if (feedback.interviewFeedback == null)
            {
                response.Status = 401;
                response.Message = "Interview Feedback with this Id does not exist.";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("EmployeeId"));
            if (!isEmployee || employeeId != feedback.interviewFeedback.EmployeeId)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to Edit Interview Feedback";
                return response;
            }

            response = await _InterviewFeedbackService.EditInterviewFeedback(interviewFeedback, employeeId);
            return response;
        }
    }
}
