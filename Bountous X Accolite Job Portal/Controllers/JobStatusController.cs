using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models;
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
        public async Task<ResponseViewModel> AddStatus(AddJobStatusViewModel addJobStatus)
        {
            ResponseViewModel response = new ResponseViewModel();

            if (!ModelState.IsValid)
            {
                response.Status = 402;
                response.Message = "Error in Values of Status !!";

                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee || !hasPrivilege || employeeId == Guid.Empty)
            {
                response.Status = 403;
                response.Message = "Not Authorized for Status !!";

                return response;
            }

            return await _jobStatusService.AddStatus(addJobStatus, employeeId);
        }

        [HttpDelete]
        [Route("removeStatus/{id}")]
        [Authorize]
        public async Task<ResponseViewModel> deleteStatus(int id)
        {
            ResponseViewModel response;
            
            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            if (!isEmployee || !hasPrivilege)
            {
                response = new ResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove Status.";
                
                return response;
            }

            return await _jobStatusService.DeleteStatus(id);
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<JobStatusResponseViewModel> GetStatusById(int Id)
        {
            JobStatusResponseViewModel response = await _jobStatusService.GetStatusById(Id);
            return response;
        }

        [HttpGet]
        [Route("getAllStatus")]
        [Authorize]
        public async Task<AllStatusResponseViewModel> GetAllStatus()
        {
            return await _jobStatusService.GetAllStatus();
        }
    }
}