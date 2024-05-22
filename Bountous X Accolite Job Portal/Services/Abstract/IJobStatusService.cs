using Bountous_X_Accolite_Job_Portal.Models.JobStatusViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJobStatusService
    {
        int GetInitialStatus();
        int getInitialReferralStatus();
        bool IsRejectedStatus(int StatusId);
        Task<bool> AddStatus(AddJobStatusViewModel status, Guid empId);
        JobStatusResponseViewModel GetStatusById(int statusId);
        AllStatusResponseViewModel GetAllStatus();
    }
}