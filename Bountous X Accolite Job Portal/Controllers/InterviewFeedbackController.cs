using Azure;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackViewModel.InterviewFeedbackResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel.InterviewResponseViewModel;
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
    public class InterviewFeedbackController : ControllerBase
    {
        private readonly I_InterviewFeedbackService _InterviewFeedbackService;
        private readonly UserManager<User> userManager;

        public InterviewFeedbackController(I_InterviewFeedbackService interviewFeedbackService, UserManager<User> userManager)
        {
            _InterviewFeedbackService = interviewFeedbackService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("getAllInterviewFeedbacks")]
        public async Task<AllInterviewFeedbackResponseViewModel> GetAllInterviewFeedbacks()
        {
            AllInterviewFeedbackResponseViewModel response = new AllInterviewFeedbackResponseViewModel();
            var emp = await userManager.GetUserAsync(User);

            if(emp==null || emp.EmpId==null)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to See All Interviews Feedbacks";
            }
            else
            {
                response = _InterviewFeedbackService.GetAllInterviewFeedbacks();
            }

            return response;
        }

        [HttpGet]
        [Route("GetInterviewFeedbackById/{Id}")]
        public async Task<InterviewFeedbackResponseViewModel> GetInterviewFeedbackById(Guid Id)
        {
            InterviewFeedbackResponseViewModel response = new InterviewFeedbackResponseViewModel();
            var emp = await userManager.GetUserAsync(User);

            if (emp == null || emp.EmpId == null)
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

            var emp = await userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to Add Interview Feedback";
                return response;
            }

            response = await _InterviewFeedbackService.AddInterviewFeedback(interviewFeedback,(Guid)emp.EmpId);
            return response;
        }

        [HttpDelete]
        [Route("DeleteInterviewFeedback")]
        public async Task<InterviewFeedbackResponseViewModel> DeleteInterviewFeedback(Guid Id)
        {
            InterviewFeedbackResponseViewModel response = new InterviewFeedbackResponseViewModel();

            var emp = await userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
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

            var emp = await userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to Edit Interview Feedback";
                return response;
            }

            response = await _InterviewFeedbackService.EditInterviewFeedback(interviewFeedback, (Guid)emp.EmpId);
            return response;
        }
    }
}
