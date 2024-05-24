using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel.JobCategoryResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCategoryController : ControllerBase
    {
        private readonly IJobCategoryService _jobCategory;
        public JobCategoryController(IJobCategoryService jobCategory)
        {
            _jobCategory = jobCategory;
        }

        [HttpGet]
        [Route("getAllJobCategory")]
        public async Task<AllJobCategoryResponseViewModel> GetAllJobCategory()
        {
            return await _jobCategory.GetAllJobCategory();
        }

        [HttpGet]
        [Route("getJobCategory/{Id}")]
        public async Task<JobCategoryResponseViewModel> GetJobCategoryById(Guid Id)
        {
            return await _jobCategory.GetJobCategoryById(Id); 
        }

        [HttpPost]
        [Route("AddJobCategory")]
        [Authorize]
        public async Task<JobCategoryResponseViewModel> AddJobCategory(CreateJobCategoryViewModel category)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();
            if(!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee || !hasPrivilege || employeeId == Guid.Empty)
            {
                response.Status = 403;
                response.Message = "Not Logged IN / Not Authorized to Add Category";
                return response;
            }
            
            response = await _jobCategory.AddJobCategory(category, employeeId);
            return response;
        }

        [HttpPut]
        [Route("UpdateJobCategory")]
        [Authorize]
        public async Task<JobCategoryResponseViewModel> UpdateJobCategory(EditJobCategoryViewModel category)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();
            if (!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            if (!isEmployee || !hasPrivilege)
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Edit Category";
                return response;
            }

            response = await _jobCategory.UpdateJobCategory(category);
            return response;
        }

        [HttpDelete]
        [Route("DeleteJobCategory/{Id}")]
        [Authorize]
        public async Task<JobCategoryResponseViewModel> DeleteJobCategory(Guid Id)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            bool hasPrivilege = Convert.ToBoolean(User.FindFirstValue("HasPrivilege"));
            if (!isEmployee || !hasPrivilege)
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Delete Category";
                return response;
            }

            response = await _jobCategory.DeleteJobCategory(Id);
            return response;
        }
    }
}
