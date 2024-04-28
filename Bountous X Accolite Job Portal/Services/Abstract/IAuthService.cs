using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterViewModel registerUser);
        Task<bool> Login(UserLoginViewModel loginUser);
    }
}
