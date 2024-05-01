using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;
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
    private readonly IJobApplicationService _jobApplicationService;
    public ApplicationController(UserManager<User> userManager, IJobApplicationService applicationService)
    {
        _userManager = userManager;
        _jobApplicationService = applicationService;
    }

    [HttpPost]
    [Route("apply")]
    public async Task<JobApplicationResponseViewModel> Apply(AddJobApplication addjobapplication)
    {
        JobApplicationResponseViewModel response;

        if (!ModelState.IsValid)
        {
            response = new JobApplicationResponseViewModel();
            response.Status = 400;
            response.Message = "PLease fill all the details.";
        }

        var user = await _userManager.GetUserAsync(User);
        if(user == null || user.EmpId != null)
        {
            response = new JobApplicationResponseViewModel();
            response.Status = 403;
            response.Message = "You are not loggedIn or you are not authorised to apply this job.";
        }
        
        response = await _jobApplicationService.Apply(addjobapplication, (Guid)user.CandidateId);
        return response;
    }
        
    }
