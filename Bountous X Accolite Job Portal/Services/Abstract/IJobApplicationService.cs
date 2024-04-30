using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJobApplicationService
    {
        Task<bool> AddApplications(JobApplicationViewModel jobApplication);


    }
}
