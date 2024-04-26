using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> AddDesignation(AddDesignationViewModel addDesignation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please Enter all details.");
            }

            var user = await _userManager.GetUserAsync(User);
            if(user == null || user.EmpId == null)
            {
                return BadRequest("You are not authorized to add Designation.");  
            }

            var isAdded = await _designationService.AddDesignation(addDesignation, (Guid)user.EmpId);
            if (isAdded)
            {
                return Ok("Designation successfully added.");
            }

            return BadRequest("Could not register designation.");
        }
    }
}
