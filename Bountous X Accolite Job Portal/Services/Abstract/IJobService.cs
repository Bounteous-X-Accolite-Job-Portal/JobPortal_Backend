using Bountous_X_Accolite_Job_Portal.Models.ClosedJobViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJobService
    {
        Task<AllJobResponseViewModel> GetAllJobs();
        AllJobResponseViewModel GetAllJobsByEmployeeId(Guid Emp);
        JobResponseViewModel GetJobById(Guid id);
        AllClosedJobResponseViewModel GetAllClosedJobsByEmployeeId(Guid Emp);
        ClosedJobResponseViewModel GetClosedJobById(Guid id);
        Task<JobResponseViewModel> AddJob(CreateJobViewModel job, Guid EmpId);
        Task<JobResponseViewModel> EditJob(EditJobViewModel job);
        Task<JobResponseViewModel> DeleteJob(Guid JobId);
    }
}
