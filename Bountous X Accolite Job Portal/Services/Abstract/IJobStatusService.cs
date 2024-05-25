using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobStatusViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJobStatusService
    {
        Task<int> GetInitialApplicationStatus();
        Task<int> getInitialSuccesstatus();
        Task<ResponseViewModel> AddStatus(AddJobStatusViewModel status, Guid empId);
        Task<ResponseViewModel> DeleteStatus(int statusId);
        Task<int> getInitialReferralStatus();
        Task<bool> IsRejectedStatus(int StatusId);
        Task<JobStatusResponseViewModel> GetStatusById(int statusId); 
        Task<AllStatusResponseViewModel> GetAllStatus();
    }
}