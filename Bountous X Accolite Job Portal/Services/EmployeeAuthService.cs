using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class EmployeeAuthService : IEmployeeAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public EmployeeAuthService(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = applicationDbContext;
        }

        public async Task<bool> Login(EmployeeLoginViewModel employee)
        {
            if(employee.Email == null || employee.Password == null)
            {
                return false;
            }

            var result = await _signInManager.PasswordSignInAsync(employee.Email, employee.Password, employee.RememberMe, lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task<bool> Register(EmployeeRegisterViewModel RegisterEmployee)
        {
            var employee = new Employee();
            employee.EmpId = RegisterEmployee.EmpId;  // company employee id
            employee.FirstName = RegisterEmployee.FirstName;
            employee.LastName = RegisterEmployee.LastName;
            employee.Email = RegisterEmployee.Email;
            employee.DesignationId = RegisterEmployee.DesignationId;

            await _dbContext.Employees.AddAsync(employee);

            var user = new User();
            user.UserName = employee.Email;
            user.Email = employee.Email;
            user.EmpId = employee.EmployeeId;

            var result = await _userManager.CreateAsync(user, RegisterEmployee.Password);
            if (!result.Succeeded)
            {
                _dbContext.Employees.Remove(employee);
                user = null;
            }

            if (user == null)
            {
                return false;
            }

            await _dbContext.SaveChangesAsync();
            // TODO: return status codes for further error handling
            return true;
        }
    }
}
