using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateAccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ICandidateAccountService _candidateAccountService;
        public CandidateAccountController(ICandidateAccountService candidateAccountService, UserManager<User> userManager)
        {
            _candidateAccountService = candidateAccountService; 
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<CandidateResponseViewModel> Register(CandidateRegisterViewModel candidate)
        {
            CandidateResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new CandidateResponseViewModel();
                response.Status = 404;
                response.Message = "Please fill all the details.";
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                response = new CandidateResponseViewModel();
                response.Status = 403;
                response.Message = "Please first logout to login.";
                return response;
            }

            response = await _candidateAccountService.Register(candidate);
            return response;
        }

    }
}
