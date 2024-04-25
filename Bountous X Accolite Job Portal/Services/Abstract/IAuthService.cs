namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IAuthService
    {
        Task<bool> Register(string FirstName, string LastName, string Email, string Password);
        Task<bool> Login(string Email, string Password);
    }
}
