using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels;
using Bountous_X_Accolite_Job_Portal.Models.EMAIL;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface ICandidateAccountService
    {
        Task<CandidateResponseViewModel> Register(CandidateRegisterViewModel registerUser);
        void SendConfirmEmail(EmailData email);
    }
}
