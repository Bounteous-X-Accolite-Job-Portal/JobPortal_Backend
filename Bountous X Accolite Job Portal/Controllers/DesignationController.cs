using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesignationController : ControllerBase
    {
        private readonly IDesignationService _designationService;
        public DesignationController(IDesignationService designationService)
        {
            _designationService = designationService;   
        }

        [HttpGet]
        [Route("getAllDesignations")]
        public AllDesignationResponseViewModel GetAllDesignations()
        {
            return _designationService.GetAllDesignation();
        }

        [HttpGet]
        [Route("designation/{Id}")]
        public async Task<DesignationResponseViewModel> GetDesignationsById(int Id)
        {
            return await _designationService.GetDesignationById(Id);
        }

        [HttpPost]
        [Route("addDesignation")]
        public async Task<DesignationResponseViewModel> AddDesignation(AddDesignationViewModel addDesignation)
        {
            DesignationResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new DesignationResponseViewModel();
                response.Status = 404;
                response.Message = "Please Enter all details.";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid employeeId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            var role = User.FindFirstValue("Role");
            if (!isEmployee || role == null || !_designationService.HasPrivilege(role))
            {
                response = new DesignationResponseViewModel();
                response.Status = 403;
                response.Message = "You are not authorized to add Designation.";
                return response;  
            }

            response = await _designationService.AddDesignation(addDesignation, employeeId);
            return response;
        }
    }
}
