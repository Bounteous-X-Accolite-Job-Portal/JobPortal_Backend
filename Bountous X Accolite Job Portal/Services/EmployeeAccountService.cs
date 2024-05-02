using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class EmployeeAccountService : IEmployeeAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public EmployeeAccountService(UserManager<User> userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _dbContext = applicationDbContext;
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
