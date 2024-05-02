using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateAccountController : ControllerBase
    {
        private readonly ICandidateAccountService _candidateAccountService;
        public CandidateAccountController(ICandidateAccountService candidateAccountService)
        {
            _candidateAccountService = candidateAccountService; 
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

            var email = User.FindFirstValue(ClaimTypes.Name);
            if (email != null)
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
