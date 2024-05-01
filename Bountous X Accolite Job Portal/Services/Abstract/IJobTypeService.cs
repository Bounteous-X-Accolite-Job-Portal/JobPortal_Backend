using Bountous_X_Accolite_Job_Portal.Models.JobTypeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobTypeViewModel.JobTypeResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJobTypeService
    {
        AllJobTypeResponseViewModel GetAllJobTypes();
        JobTypeResponseViewModel GetJobTypeById(Guid Id);
        Task<JobTypeResponseViewModel> AddJobType(CreateJobTypeViewModel jobType, Guid EmpId);
        Task<JobTypeResponseViewModel> UpdateJobType(EditJobTypeViewModel jobType);
        Task<JobTypeResponseViewModel> DeleteJobType(Guid Id);
    }
}
