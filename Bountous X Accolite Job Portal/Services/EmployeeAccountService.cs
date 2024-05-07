using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class EmployeeAccountService : IEmployeeAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IDesignationService _designationService;

        public EmployeeAccountService(UserManager<User> userManager, ApplicationDbContext applicationDbContext, IDesignationService designationService)
        {
            _userManager = userManager;
            _dbContext = applicationDbContext;
            _designationService = designationService;
        }

        public AllEmployeesResponseViewModel GetAllEmployees()
        {
            AllEmployeesResponseViewModel response = new AllEmployeesResponseViewModel();

            List<Employee> listOfEmployee = _dbContext.Employees.ToList();

            List<EmployeeViewModels> employees = new List<EmployeeViewModels>();
            foreach(Employee employee in listOfEmployee) 
            { 
                employees.Add(new EmployeeViewModels(employee));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all employees";
            response.Employees = employees;
            return response;
        }

        public async Task<EmployeeResponseViewModel> DisableEmployeeAccount(Guid EmployeeId, bool HasSpecialPrivilege)
        {
            EmployeeResponseViewModel response = new EmployeeResponseViewModel();

            var employee = _dbContext.Employees.Find(EmployeeId);
            if (employee == null)
            {
                response.Status = 404;
                response.Message = "Employee with this Id does not exist.";
                return response;
            }

            var designation = _dbContext.Designations.Find(employee.DesignationId);
            if (designation == null)
            {
                response.Status = 404;
                response.Message = "Designation with this Id does not exist.";
                return response;
            }

            var checkSpecialPriviledge = _designationService.HasSpecialPrivilege(designation.DesignationName);
            if(checkSpecialPriviledge && !HasSpecialPrivilege)
            {
                response.Status = 401;
                response.Message = "You are not authorised to diable this account.";
                return response;
            }

            employee.Inactive = true;

            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully diabled the account.";
            response.Employee = new EmployeeViewModels(employee);
            return response;
        }

        public async Task<EmployeeResponseViewModel> Register(EmployeeRegisterViewModel RegisterEmployee)
        {
            EmployeeResponseViewModel response = new EmployeeResponseViewModel();

            var checkUserWhetherExist = _dbContext.Users.Where(item => item.Email == RegisterEmployee.Email).ToList();
            if (checkUserWhetherExist.Count != 0)
            {
                response.Status = 409;
                response.Message = "This email is already registered with us. Please Login.";
                return response;
            }

            var employee = new Employee();
            employee.EmpId = RegisterEmployee.EmpId;  // company employee id
            employee.FirstName = RegisterEmployee.FirstName;
            employee.LastName = RegisterEmployee.LastName;
            employee.Email = RegisterEmployee.Email;
            employee.DesignationId = RegisterEmployee.DesignationId;
            employee.Inactive = false;

            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            if(employee == null)
            {
                response.Status = 500;
                response.Message = "Unable to create Employee, please try again.";
                return response;
            }

            var user = new User();
            user.UserName = employee.Email;
            user.Email = employee.Email;
            user.EmpId = employee.EmployeeId;

            var result = await _userManager.CreateAsync(user, RegisterEmployee.Password);
            if (!result.Succeeded)
            {
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();

                response.Status = 500;
                response.Message = "Unable to create Employee, please try again.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully created employee.";
            response.Employee = new EmployeeViewModels(employee);
            return response;
        }
    }
}
