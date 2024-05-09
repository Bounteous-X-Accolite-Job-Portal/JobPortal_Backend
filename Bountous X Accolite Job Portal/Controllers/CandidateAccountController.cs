using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels;
using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel.ResponseViewModels;
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

        [HttpGet]
        [Route("{Id}")]
        public CandidateResponseViewModel GetCandidateById(Guid Id)
        {
            CandidateResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee && candidateId != Id)
            {
                response = new CandidateResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to get candidate details.";
                return response;
            }

            response = _candidateAccountService.GetCandidateById(Id);
            return response;
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

            var email = User.FindFirstValue("Name");
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
