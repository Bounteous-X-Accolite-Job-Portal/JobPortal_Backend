using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.JobStatusViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobStatusController : Controller
    {
        private readonly IJobStatusService _jobStatusService;
        public JobStatusController(IJobStatusService jobStatusService)
        {
            _jobStatusService = jobStatusService;
        }

        [HttpPost]
        [Route("addJobStatus")]
        [Authorize]
        public async Task<IActionResult> AddStatus(AddJobStatusViewModel addJobStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please Enter all details.");
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("EmployeeId"));
            if (!isEmployee || employeeId == Guid.Empty)
            {
                return BadRequest(new { Message = "You are not authorized to add Status." });
            }

            var isAdded = await _jobStatusService.AddStatus(addJobStatus, employeeId);
            if (isAdded)
            {
                return Ok("Status successfully added.");
            }

            return BadRequest("Could not register Status.");
        }



    }
}
