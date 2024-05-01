using Azure;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobStatusViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobStatusController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IJobStatusService _jobStatusService;
        public JobStatusController(UserManager<User> userManager, IJobStatusService jobStatusService)
        {
            _userManager = userManager;
            _jobStatusService = jobStatusService;
        }

        [HttpPost]
        [Route("addJobStatus")]
        public async Task<IActionResult> AddStatus(AddJobStatusViewModel addJobStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please Enter all details.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId == null)
            {
                return BadRequest("You are not authorized to add Status.");
            }

            var isAdded = await _jobStatusService.AddStatus(addJobStatus, (Guid)user.EmpId);
            if (isAdded)
            {
                return Ok("Status successfully added.");
            }

            return BadRequest("Could not register Status.");
        }



    }
}
