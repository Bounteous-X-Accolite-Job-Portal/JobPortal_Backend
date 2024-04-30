using Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel.JobCategoryResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJobCategoryService
    {
        AllJobCategoryResponseViewModel GetAllJobCategory();
        JobCategoryResponseViewModel GetJobCategoryById(Guid id);
        Task<JobCategoryResponseViewModel> AddJobCategory(CreateJobCategoryViewModel location , Guid EmpId);
        Task<JobCategoryResponseViewModel> UpdateJobCategory(EditJobCategoryViewModel location);
        Task<JobCategoryResponseViewModel> DeleteJobCategory(Guid locationId);
    }
}
