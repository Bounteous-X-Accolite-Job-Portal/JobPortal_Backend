using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.JobPositionViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobPositionViewModel.JobPositionResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPositionController : ControllerBase
    {
        private readonly IJobPositionService _jobPosition;
        private readonly IDesignationService _designationService;
        public JobPositionController(IJobPositionService jobPosition, IDesignationService designationService)
        {
            _jobPosition = jobPosition;
            _designationService = designationService;
        }

        [HttpGet]
        [Route("getAllJobPositions")]
        public AllJobPositionResponseViewModel GetAllJobPositions()
        {
            return _jobPosition.GetAllJobPositions();
        }

        [HttpGet]
        [Route("getJobPosition/{Id}")]
        public JobPositionResponseViewModel GetJobPositionById(Guid Id)
        {
            return _jobPosition.GetJobPositionById(Id);
        }

        [HttpPost]
        [Route("AddJobPosition")]
        [Authorize]
        public async Task<JobPositionResponseViewModel> AddJobPosition(CreateJobPositionViewModel jobPosition)
        {
            JobPositionResponseViewModel response = new JobPositionResponseViewModel();
            if(!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role) || employeeId == Guid.Empty)
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Add Position";
                return response;
            }

            response = await _jobPosition.AddJobPosition(jobPosition, employeeId);
            return response;
        }

        [HttpPut]
        [Route("UpdateJobPosition")]
        [Authorize]
        public async Task<JobPositionResponseViewModel> UpdateJobPosition(EditJobPositionViewModel jobPosition)
        {
            JobPositionResponseViewModel response = new JobPositionResponseViewModel();
            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role))
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Update Position";
                return response;
            }

            response = await _jobPosition.UpdateJobPosition(jobPosition);
            return response;
        }

        [HttpDelete]
        [Route("DeleteJobPosition/{Id}")]
        [Authorize]
        public async Task<JobPositionResponseViewModel> DeleteJobPosition(Guid PositionId)
        {
            JobPositionResponseViewModel response = new JobPositionResponseViewModel();

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role))
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Delete Position";
                return response;
            }

            response = await _jobPosition.DeleteJobPosition(PositionId);
            return response;
        }
    }
}
