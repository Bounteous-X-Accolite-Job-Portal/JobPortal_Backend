using Bountous_X_Accolite_Job_Portal.Models.JobViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJob 
    {
        AllJobResponseViewModel GetAllJobs();
        JobResponseViewModel GetJobById(Guid id);
        Task<JobResponseViewModel> AddJob(CreateJobViewModel job, Guid EmpId);
        Task<JobResponseViewModel> EditJob(EditJobViewModel job);
        Task<JobResponseViewModel> DeleteJob(Guid JobId);
    }
}
