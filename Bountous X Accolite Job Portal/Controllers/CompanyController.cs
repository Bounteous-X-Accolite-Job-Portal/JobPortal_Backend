using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.CompanyViewModel;
using Bountous_X_Accolite_Job_Portal.Models.CompanyViewModel.CompanyResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DegreeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DegreeViewModel.DegreeResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly UserManager<User> _userManager;
        public CompanyController(ICompanyService companyService, UserManager<User> userManager)
        {
            _companyService = companyService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("getAllCompanies")]
        public AllCompanyResponseViewModel GetAllCompanies()
        {
            AllCompanyResponseViewModel response = _companyService.GetAllCompanies();   
            return response;
        }

        [HttpGet]
        [Route("getCompany/{Id}")]
        public CompanyResponseViewModel GetCompanyById(Guid Id)
        {
            CompanyResponseViewModel response = _companyService.GetCompanyById(Id); 
            return response;
        }

        [HttpPost]
        [Route("addCompany")]
        public async Task<CompanyResponseViewModel> AddCompany(AddCompanyViewModel addCompany)
        {
            CompanyResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new CompanyResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            var employee = await _userManager.GetUserAsync(User);
            if (employee == null || employee.EmpId == null)
            {
                response = new CompanyResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add Company.";
                return response;
            }

            response = await _companyService.AddCompany(addCompany, (Guid)employee.EmpId);
            return response;
        }

        [HttpPost]
        [Route("updateCompany")]
        public async Task<CompanyResponseViewModel> UpdateCompany(UpdateCompanyViewModel updateCompany)
        {
            CompanyResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new CompanyResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            var employee = await _userManager.GetUserAsync(User);
            if (employee == null || employee.EmpId == null)
            {
                response = new CompanyResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update Company.";
                return response;
            }

            response = await _companyService.UpdateCompany(updateCompany);
            return response;
        }

        [HttpPost]
        [Route("removeCompany/{id}")]
        public async Task<CompanyResponseViewModel> RemoveCompany(Guid id)
        {
            CompanyResponseViewModel response;

            var employee = await _userManager.GetUserAsync(User);
            if (employee == null || employee.EmpId == null)
            {
                response = new CompanyResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove Company.";
                return response;
            }

            response = await _companyService.RemoveCompany(id);
            return response;
        }

    }
}
