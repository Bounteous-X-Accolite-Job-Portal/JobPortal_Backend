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
        private readonly IDesignationService _designationService;
        public JobStatusController(IJobStatusService jobStatusService, IDesignationService designationService)
        {
            _jobStatusService = jobStatusService;
            _designationService = designationService;
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
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role) || employeeId == Guid.Empty)
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

        [HttpGet]
        [Route("{Id}")]
        public JobStatusResponseViewModel GetStatusById(int Id)
        {
            JobStatusResponseViewModel response = _jobStatusService.GetStatusById(Id);
            return response;
        }


    }
}
