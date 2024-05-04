using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel;
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
        public ApplicationController(IJobApplicationService applicationService)
        {
            _jobApplicationService = applicationService;
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
        
    }
}
