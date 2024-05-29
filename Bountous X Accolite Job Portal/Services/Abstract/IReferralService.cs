using Bountous_X_Accolite_Job_Portal.Models.EMAIL;
using Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel;
using Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel.ResponseViewModels;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IReferralService
    {
        Task<AllReferralResponseViewModel> GetAllReferralsOfLoggedInEmployee(Guid EmpId);
        Task<ReferralResponseViewModel> Refer(AddReferralViewModel addReferral, Guid EmpId);
        Task<bool> AddApplicationIdToReferral(Guid ReferralId, Guid ApplicationId);
        Task<bool> AddClosedApplicationIdToReferral(Guid ReferralId, Guid ClosedApplicationId);
        Task<bool> AddClosedJobIdToReferral(Guid ReferralId, Guid ClosedJobId);
    }
}
