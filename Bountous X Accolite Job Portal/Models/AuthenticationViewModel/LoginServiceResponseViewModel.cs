using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel;

namespace Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel
{
    public class LoginServiceResponseViewModel : ResponseViewModel
    {
        public string? Token { get; set; }
        public CandidateViewModel? Candidate { get; set; }
        public EmployeeViewModels? Employee { get; set; }
        public User? User { get; set; }
    }
}
