using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.DegreeModels.DegreeResponseViewModel;
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
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role) || employeeId == Guid.Empty)
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
            ResponseViewModel response = new ResponseViewModel();
            
            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role))
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
        public JobStatusResponseViewModel GetStatusById(int Id)
        {
            JobStatusResponseViewModel response = _jobStatusService.GetStatusById(Id);
            return response;
        }

        [HttpGet]
        [Route("getAllStatus")]
        [Authorize]
        public AllStatusResponseViewModel GetAllStatus()
        {
            return _jobStatusService.GetAllStatus();
        }
    }
}