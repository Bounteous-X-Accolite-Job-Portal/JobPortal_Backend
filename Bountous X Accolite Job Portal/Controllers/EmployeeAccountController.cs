﻿using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAccountController : ControllerBase
    {
        private readonly IEmployeeAccountService _employeeAuthService;
        private readonly UserManager<User> _userManager;
        public EmployeeAccountController(IEmployeeAccountService employeeAuthService, UserManager<User> userManager)
        {
            _employeeAuthService = employeeAuthService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<EmployeeResponseViewModel> Register(EmployeeRegisterViewModel employee)
        {
            EmployeeResponseViewModel response;

            if(!ModelState.IsValid)
            {
                response = new EmployeeResponseViewModel();
                response.Status = 404;
                response.Message = "Please fill all the details.";
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            if(user != null)
            {
                response = new EmployeeResponseViewModel();
                response.Status = 403;
                response.Message = "Please first logout to login.";
                return response;
            }

            response = await _employeeAuthService.Register(employee);
            return response;
        }

    }
}
