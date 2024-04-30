using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.EducationInstitutionViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EducationInstitutionController : ControllerBase
    {
        private readonly IEducationInstitutionService _educationInstitutionService;
        private readonly UserManager<User> _userManager;
        public EducationInstitutionController(IEducationInstitutionService educationInstitutionService, UserManager<User> userManager)
        {
            _educationInstitutionService = educationInstitutionService; 
            _userManager = userManager;
        }

        [HttpGet]
        [Route("getAllInstitution")]
        public async Task<AllInstitutionResponseViewModel> GetAllInstitution()
        {
            List<InstitutionViewModel> instutions = _educationInstitutionService.GetAllInstitutions();

            AllInstitutionResponseViewModel response = new AllInstitutionResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully retrieved all institutions.";
            response.EducationInstitution = instutions;
            return response;
        }

        [HttpGet]
        [Route("getInstitution/{id}")]
        public async Task<InstitutionResponseViewModel> GetInstitution(Guid id)
        {
            InstitutionResponseViewModel response = _educationInstitutionService.GetInstitution(id);
            return response;
        }

        [HttpPost]
        [Route("addInstitution")]
        public async Task<InstitutionResponseViewModel> AddInstution(AddInstitutionViewModel addInstitution)
        {
            InstitutionResponseViewModel response;

            if(!ModelState.IsValid)
            {
                response = new InstitutionResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            var employee = await _userManager.GetUserAsync(User);
            if (employee == null || employee.EmpId == null)
            {
                response = new InstitutionResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add Institutions.";
                return response;
            }

            response = await _educationInstitutionService.AddInstitution(addInstitution, (Guid)employee.EmpId);
            return response;
        }

        [HttpPut]
        [Route("updateInstution")]
        public async Task<InstitutionResponseViewModel> UpdateInstitution(UpdateInstitutionViewModel updateInstitution)
        {
            InstitutionResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new InstitutionResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            var employee = await _userManager.GetUserAsync(User);
            if (employee == null || employee.EmpId == null)
            {
                response = new InstitutionResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update Institutions.";
                return response;
            }

            response = await _educationInstitutionService.UpdateInstution(updateInstitution);
            return response;    
        }

        [HttpDelete]
        [Route("removeInstitution/{id}")]
        public async Task<InstitutionResponseViewModel> RemoveInstitution(Guid id)
        {
            InstitutionResponseViewModel response;

            var employee = await _userManager.GetUserAsync(User);
            if (employee == null || employee.EmpId == null)
            {
                response = new InstitutionResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove Institutions.";
                return response;
            }

            response = await _educationInstitutionService.RemoveInstitution(id);
            return response;
        }
    }
}
