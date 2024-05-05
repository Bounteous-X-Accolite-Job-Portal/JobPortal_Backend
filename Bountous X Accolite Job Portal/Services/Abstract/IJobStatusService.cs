using Bountous_X_Accolite_Job_Portal.Models.JobStatusViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJobStatusService
    {
        int GetInitialStatus();
        bool IsRejectedStatus(int StatusId);
        public  Task<bool> AddStatus(AddJobStatusViewModel status, Guid empId);
        
    }
}
