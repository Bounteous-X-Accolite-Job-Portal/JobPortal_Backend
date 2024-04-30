using Bountous_X_Accolite_Job_Portal.Models;
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
        [Route("getInterviewForCandidate/{Id}")]
        public async Task<All_InterviewResponseViewModel> GetInterviewForCandidate()
        {
            All_InterviewResponseViewModel response = new All_InterviewResponseViewModel();
            
            
            return response;
        }
    }
}
