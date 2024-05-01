using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel.InterviewResponseViewModel;
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
    public class InterviewController : ControllerBase
    {
        private readonly I_InterviewService _InterviewService;
        private readonly UserManager<User> userManager;
        public InterviewController(I_InterviewService interviewService, UserManager<User> userManager)
        {
            _InterviewService = interviewService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("getAllInterviews")]
        public async Task<All_InterviewResponseViewModel> GetAllInterviews()
        {
            All_InterviewResponseViewModel response = new All_InterviewResponseViewModel();
            var emp = await userManager.GetUserAsync(User);

            if(emp==null || emp.EmpId==null)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to See All Interviews";
            }
            else
            {
                response = _InterviewService.GetAllInterviews();
            }
            return response;
        }

        [HttpGet]
        [Route("GetAllInterviewsForInterviewer/{Id}")]
        public async Task<All_InterviewResponseViewModel> GetAllInterviewsForInterviewer()
        {
            All_InterviewResponseViewModel response = new All_InterviewResponseViewModel();
            var emp = await userManager.GetUserAsync(User);

            if(emp==null || emp.EmpId==null)
            {
                response.Status = 401;
                response.Message = "Not Logged In / Not Authorized to See Interviewer Interviews !";
            }
            else
            {
                response = _InterviewService.GetAllInterviewsForInterviewer((Guid)emp.EmpId);
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
            var emp = await userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
            {
                response = new InterviewResponseViewModel();
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Add Interview";
                return response;
            }

            response = await _InterviewService.AddInterview(interview,(Guid)emp.EmpId);
            return response;
        }

        [HttpDelete]
        [Route("DeleteInterview")]
        public async Task<InterviewResponseViewModel> DeleteInterview(Guid Id)
        {
            InterviewResponseViewModel response;
            var emp = await userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
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
            var emp = await userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
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
