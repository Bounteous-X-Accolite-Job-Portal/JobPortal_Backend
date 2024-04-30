using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel;
using Bountous_X_Accolite_Job_Portal.Services;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{

}
[Route("api/[controller]")]
[ApiController]
public class ApplicationController : ControllerBase
{

    private readonly UserManager<User> _userManager;
    private readonly IApplicationService _applicationService;
    public ApplicationController(UserManager<User> userManager, IApplicationService applicationService)
    {
        _userManager = userManager;
        _applicationService = applicationService;
    }

    [HttpPost]
    [Route("addApplication")]
    public async Task<IActionResult> AddApplication(JobApplicationViewModel addjobapplication)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Please Enter all details.");
        }
        if (addjobapplication == null )
        {
            return BadRequest("Please Enter all feilds to Login.");
        }

        var jobbapplication = await _applicationService.AddApplications(addjobapplication, jobApplication);
        if(jobbapplication)
            {
            return Ok("Job Application Added");
        }

        return BadRequest("Not Added");

    }
        
    }
