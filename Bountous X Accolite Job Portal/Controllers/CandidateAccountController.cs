using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels;
using Bountous_X_Accolite_Job_Portal.Models.EMAIL;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateAccountController : ControllerBase
    {
        private readonly ICandidateAccountService _candidateAccountService;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _authContext;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        public CandidateAccountController(ICandidateAccountService candidateAccountService,UserManager<User> userManager,IConfiguration configuration,IEmailService emailService,ApplicationDbContext applicationDb)
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