using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IEmployeeAccountService
    {
        EmployeeResponseViewModel GetEmployeeById(Guid Id);
        AllEmployeesResponseViewModel GetAllEmployees();
        Task<EmployeeResponseViewModel> DisableEmployeeAccount(Guid EmployeeId, bool HasSpecialPrivilege);
        Task<EmployeeResponseViewModel> Register(EmployeeRegisterViewModel employee);
    }
}
