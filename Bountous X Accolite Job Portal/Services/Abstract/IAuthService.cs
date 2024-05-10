using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IAuthService
    {
        Task<LoginServiceResponseViewModel> Login(LoginViewModel loginUser);
        Task<ResponseViewModel> Logout();
    }
}
