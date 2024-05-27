using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IEmployeeAccountService
    {
        Task<EmployeeResponseViewModel> GetEmployeeById(Guid Id);
        Task<AllEmployeesResponseViewModel> GetAllEmployees();
        Task<EmployeeResponseViewModel> ToggleEmployeeAccountStatus(Guid EmployeeId, bool HasSpecialPrivilege);
        Task<EmployeeResponseViewModel> Register(EmployeeRegisterViewModel employee);
    }
}
