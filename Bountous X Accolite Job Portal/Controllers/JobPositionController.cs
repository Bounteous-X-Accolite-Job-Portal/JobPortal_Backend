using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobPositionViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobPositionViewModel.JobPositionResponseViewModel;
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
    public class JobPositionController : ControllerBase
    {
        private readonly IJobPosition _jobPosition;
        private readonly UserManager<User> _userManager;

        public JobPositionController(IJobPosition jobPosition, UserManager<User> userManager)
        {
            _jobPosition = jobPosition;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("getAllJobPositions")]
        public AllJobPositionResponseViewModel GetAllJobPositions()
        {
            return _jobPosition.GetAllJobPositions();
        }

        [HttpGet]
        [Route("getJobPosition/{Id}")]
        public JobPositionResponseViewModel GetJobPositionById(Guid PositionId)
        {
            return _jobPosition.GetJobPositionById(PositionId);
        }

        [HttpPost]
        [Route("AddJobPosition")]
        public async Task<JobPositionResponseViewModel> AddJobPosition(CreateJobPositionViewModel jobPosition)
        {
            JobPositionResponseViewModel response = new JobPositionResponseViewModel();
            if(!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            var emp = await _userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Add Position";
                return response;
            }

            response = await _jobPosition.AddJobPosition(jobPosition,(Guid)emp.EmpId);
            return response;
        }

        [HttpPut]
        [Route("UpdateJobPosition")]
        public async Task<JobPositionResponseViewModel> UpdateJobPosition(EditJobPositionViewModel jobPosition)
        {
            JobPositionResponseViewModel response = new JobPositionResponseViewModel();
            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            var emp = await _userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
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
        public async Task<JobPositionResponseViewModel> DeleteJobPosition(Guid PositionId)
        {
            JobPositionResponseViewModel response = new JobPositionResponseViewModel();
            
            var emp = await _userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
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
