using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.CompanyViewModel;
using Bountous_X_Accolite_Job_Portal.Models.CompanyViewModel.CompanyResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IDesignationService _designationService;
        public CompanyController(ICompanyService companyService, IDesignationService designationService)
        {
            _companyService = companyService;
            _designationService = designationService;
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
        [Authorize]
        public CompanyResponseViewModel GetCompanyById(Guid Id)
        {
            CompanyResponseViewModel response = _companyService.GetCompanyById(Id); 
            return response;
        }

        [HttpPost]
        [Route("addCompany")]
        [Authorize]
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role))
            {
                response = new CompanyResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add Company.";
                return response;
            }

            response = await _companyService.AddCompany(addCompany, employeeId);
            return response;
        }

        [HttpPut]
        [Route("updateCompany")]
        [Authorize]
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role))
            {
                response = new CompanyResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update Company.";
                return response;
            }

            response = await _companyService.UpdateCompany(updateCompany);
            return response;
        }

        [HttpDelete]
        [Route("removeCompany/{id}")]
        [Authorize]
        public async Task<CompanyResponseViewModel> RemoveCompany(Guid id)
        {
            CompanyResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role))
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
