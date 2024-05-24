using Bountous_X_Accolite_Job_Portal.Models.JobStatusViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJobStatusService
    {
        int GetInitialStatus();
        Task<int> getInitialReferralStatus();
        Task<bool> IsRejectedStatus(int StatusId);
        Task<bool> AddStatus(AddJobStatusViewModel status, Guid empId);
        Task<JobStatusResponseViewModel> GetStatusById(int statusId); 
        Task<AllStatusResponseViewModel> GetAllStatus();
    }
}
