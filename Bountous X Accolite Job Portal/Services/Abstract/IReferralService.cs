using Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel;
using Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel.ResponseViewModels;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IReferralService
    {
        AllReferralResponseViewModel GetAllReferralsOfLoggedInEmployee(Guid EmpId);
        Task<ReferralResponseViewModel> Refer(AddReferralViewModel addReferral, Guid EmpId);
    }
}
