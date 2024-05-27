using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.EMAIL;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class EmployeeAccountService : IEmployeeAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IDesignationService _designationService;
        private readonly IDistributedCache _cache;
        private readonly IEmailService _emailService;
        public EmployeeAccountService(UserManager<User> userManager, ApplicationDbContext applicationDbContext, IDesignationService designationService, IEmailService emailService, IDistributedCache cache)
        {
            _userManager = userManager;
            _dbContext = applicationDbContext;
            _designationService = designationService;
            _cache = cache; 
            _emailService = emailService;
        }

        public async Task<AllEmployeesResponseViewModel> GetAllEmployees()
        {
            AllEmployeesResponseViewModel response = new AllEmployeesResponseViewModel();

            string key = $"allEmployees";
            string? getAllEmployeesFromCache = await _cache.GetStringAsync(key);

            List<Employee> listOfEmployee;
            if (string.IsNullOrWhiteSpace(getAllEmployeesFromCache))
            {
                listOfEmployee = _dbContext.Employees.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(listOfEmployee));
            }
            else
            {
                listOfEmployee = JsonSerializer.Deserialize<List<Employee>>(getAllEmployeesFromCache);
            }

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

        public async Task<EmployeeResponseViewModel> ToggleEmployeeAccountStatus(Guid EmployeeId, bool HasSpecialPrivilege)
        {
            EmployeeResponseViewModel response = new EmployeeResponseViewModel();

            string key = $"getEmployeeById-{EmployeeId}";
            string? getEmployeeByIdFromCache = await _cache.GetStringAsync(key);

            Employee employee;
            if (string.IsNullOrWhiteSpace(getEmployeeByIdFromCache))
            {
                employee = _dbContext.Employees.Find(EmployeeId);
                if (employee == null)
                {
                    response.Status = 404;
                    response.Message = "Employee with this Id does not exist.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(employee));
            }
            else
            {
                employee = JsonSerializer.Deserialize<Employee>(getEmployeeByIdFromCache);
            }

            var designation = await _designationService.GetDesignationById((int)employee.DesignationId);
            if (designation.Designation == null)
            {
                response.Status = 404;
                response.Message = "Designation with this Id does not exist.";
                return response;
            }

            var checkSpecialPriviledge = _designationService.HasSpecialPrivilege(designation.Designation.DesignationName);
            if(checkSpecialPriviledge && !HasSpecialPrivilege)
            {
                response.Status = 401;
                response.Message = "You are not authorised to diable this account.";
                return response;
            }

            employee.Inactive = !employee.Inactive;

            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"allEmployees");
            await _cache.RemoveAsync($"getEmployeeById-{employee.EmployeeId}");
            await _cache.RemoveAsync($"getEmployeesByDesignationId-{employee.DesignationId}");

            response.Status = 200;
            if(employee.Inactive)
            {
                response.Message = "Successfully diabled the account.";
            }
            else
            {
                response.Message = "Successfully enabled the account.";
            }
            
            response.Employee = new EmployeeViewModels(employee);
            return response;
        }

        public async Task<EmployeeResponseViewModel> Register(EmployeeRegisterViewModel RegisterEmployee)
        {
            EmployeeResponseViewModel response = new EmployeeResponseViewModel();

            string key = $"getUserByEmail-{RegisterEmployee.Email}";
            string? getUserByEmailFromCache = await _cache.GetStringAsync(key);

            User checkUserWhetherExist;
            if (string.IsNullOrWhiteSpace(getUserByEmailFromCache))
            {
                checkUserWhetherExist = _dbContext.Users.Where(item => item.Email == RegisterEmployee.Email).FirstOrDefault();
            }
            else
            {
                checkUserWhetherExist = JsonSerializer.Deserialize<User>(getUserByEmailFromCache);
            }

            var employee = new Employee();
            employee.EmpId = RegisterEmployee.EmpId;  // company employee id
            employee.FirstName = RegisterEmployee.FirstName;
            employee.LastName = RegisterEmployee.LastName;
            employee.Email = RegisterEmployee.Email;
            employee.DesignationId = RegisterEmployee.DesignationId;
            employee.Phone = RegisterEmployee.Phone;
            employee.Inactive = false;

            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
            
            if (employee == null)
            {
                response.Status = 500;
                response.Message = "Unable to create Employee, please try again.";
                return response;
            }

            var user = new User();
            user.UserName = employee.Email;
            user.Email = employee.Email;
            user.EmpId = employee.EmployeeId;

            var password = GeneratePassword.GenerateRandomPassword();
            //Console.WriteLine("Employee Password", password);

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();

                response.Status = 500;
                response.Message = "Unable to create Employee, please try again.";
                return response;
            }

            EmailData email = new EmailData(employee.Email, "bounteous x Accolite Job Portal!", EmplyoeeRegisterdMail.EmailStringBody(password));
            _emailService.SendEmail(email);

            await _cache.RemoveAsync($"allEmployees");
            await _cache.RemoveAsync($"getEmployeesByDesignationId-{employee.DesignationId}");

            response.Status = 200;
            response.Message = "Successfully created employee.";
            response.Employee = new EmployeeViewModels(employee);
            return response;
        }

        public async Task<EmployeeResponseViewModel> GetEmployeeById(Guid Id)
        {
            EmployeeResponseViewModel response = new EmployeeResponseViewModel();

            string key = $"getEmployeeById-{Id}";
            string? getEmployeeByIdFromCache = await _cache.GetStringAsync(key);

            Employee employee;
            if (string.IsNullOrWhiteSpace(getEmployeeByIdFromCache))
            {
                employee = _dbContext.Employees.Find(Id);
                if (employee == null)
                {
                    response.Status = 404;
                    response.Message = "Employee with this Id does not exist.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(employee));
            }
            else
            {
                employee = JsonSerializer.Deserialize<Employee>(getEmployeeByIdFromCache);
            }
            
            response.Status = 200;
            response.Message = "Successfully retrieved employee with this Id.";
            response.Employee = new EmployeeViewModels(employee);
            return response;
        }
    }
}
