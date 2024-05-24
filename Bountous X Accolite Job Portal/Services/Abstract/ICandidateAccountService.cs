using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels;
using Bountous_X_Accolite_Job_Portal.Models.EMAIL;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface ICandidateAccountService
    {
        CandidateResponseViewModel GetCandidateById(Guid CandidateId);
        Task<CandidateResponseViewModel> Register(CandidateRegisterViewModel registerUser);
        
        //void SendEmail(EmailData request);
    }
}
