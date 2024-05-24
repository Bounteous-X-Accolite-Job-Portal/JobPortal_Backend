using Bountous_X_Accolite_Job_Portal.Models.ClosedJobViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJobService
    {
        Task<AllJobResponseViewModel> GetAllJobs();
        Task<AllClosedJobResponseViewModel> GetAllClosedJobs();
        Task<AllJobResponseViewModel> GetAllJobsByEmployeeId(Guid Emp);
        Task<JobResponseViewModel> GetJobById(Guid id);
        Task<AllClosedJobResponseViewModel> GetAllClosedJobsByEmployeeId(Guid Emp);
        Task<ClosedJobResponseViewModel> GetClosedJobById(Guid id);
        Task<JobResponseViewModel> AddJob(CreateJobViewModel job, Guid EmpId);
        Task<JobResponseViewModel> EditJob(EditJobViewModel job);
        Task<JobResponseViewModel> DeleteJob(Guid JobId);
    }
}
