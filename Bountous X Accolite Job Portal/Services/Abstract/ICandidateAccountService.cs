using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface ICandidateAccountService
    {
        Task<CandidateResponseViewModel> Register(CandidateRegisterViewModel registerUser);
    }
}
