using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel.ResponseViewModels;
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
    public class DesignationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IDesignationService _designationService;
        public DesignationController(UserManager<User> userManager, IDesignationService designationService)
        {
            _userManager = userManager; 
            _designationService = designationService;   
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

            var user = await _userManager.GetUserAsync(User);
            if(user == null || user.EmpId == null)
            {
                response = new DesignationResponseViewModel();
                response.Status = 403;
                response.Message = "You are not authorized to add Designation.";
                return response;  
            }

            response = await _designationService.AddDesignation(addDesignation, (Guid)user.EmpId);
            return response;
        }
    }
}
