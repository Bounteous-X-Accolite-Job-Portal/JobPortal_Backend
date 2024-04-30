using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel.JobCategoryResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel.JobLocationResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services;
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
    public class JobCategoryController : ControllerBase
    {
        private readonly IJobCategory _jobCategory;
        private readonly UserManager<User> _userManager;

        public JobCategoryController(IJobCategory jobCategory, UserManager<User> userManager)
        {
            _jobCategory = jobCategory;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("getAllJobCategory")]
        public AllJobCategoryResponseViewModel GetAllJobCategory()
        {
            return _jobCategory.GetAllJobCategory();
        }

        [HttpGet]
        [Route("getJobCategory/{Id}")]
        public JobCategoryResponseViewModel GetJobCategoryById(Guid categoryId)
        {
            return _jobCategory.GetJobCategoryById(categoryId); 
        }

        [HttpPost]
        [Route("AddJobCategory")]
        public async Task<JobCategoryResponseViewModel> AddJobCategory(CreateJobCategoryViewModel category)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();
            if(!ModelState.IsValid)
            {
                response.Status = 422;
                response.Message = "Please Enter All Details";
                return response;
            }

            var emp = await _userManager.GetUserAsync(User);
            if (emp==null || emp.EmpId==null)
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Add Category";
                return response;
            }
            
            response = await _jobCategory.AddJobCategory(category,(Guid)emp.EmpId);
            return response;
        }

        [HttpPut]
        [Route("UpdateJobCategory")]
        public async Task<JobCategoryResponseViewModel> UpdateJobCategory(EditJobCategoryViewModel category)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();
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
                response.Message = "Not Logged IN / Not Authorized to Edit Category";
                return response;
            }

            response = await _jobCategory.UpdateJobCategory(category);
            return response;
        }

        [HttpDelete]
        [Route("DeleteJobCategory/{Id}")]
        public async Task<JobCategoryResponseViewModel> DeleteJobCategory(Guid categoryId)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();

            var emp = await _userManager.GetUserAsync(User);
            if (emp == null || emp.EmpId == null)
            {
                response.Status = 401;
                response.Message = "Not Logged IN / Not Authorized to Delete Category";
                return response;
            }

            response = await _jobCategory.DeleteJobCategory(categoryId);
            return response;
        }
    }
}
