using Azure;
using Bountous_X_Accolite_Job_Portal.Data;
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
            _userManager = userManager;
            _config = configuration;
            _authContext= applicationDb;
            _emailService = emailService;
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


        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> SendEmail(string email, IConfiguration _config)
        {
            var user=await _userManager.FindByEmailAsync(email);
            if (user != null)
            {

                if (user == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "User Not Found"
                    });
                }
                var tokenBytes = RandomNumberGenerator.GetBytes(64);
                    var emailToken = Convert.ToBase64String(tokenBytes);
                user.EmailToken = emailToken;
                user.EmailConfirmExpiry = DateTime.Now.AddMinutes(5);

                string from = _config["EmailSettings:From"];
                    var emailModel = new EmailData(email, "Confirm Email", ConfirmEmailBody.EmailStringBody(email, emailToken));
                    _emailService.SendEmail(emailModel);

                    _authContext.Entry(user).State = EntityState.Modified;
                    await _authContext.SaveChangesAsync();
                    


                }
            return Ok();

        }
            [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmEmail(ConfirmPasswordDTO confirm)
        {
            var newToken = confirm.EmailToken.Replace(" ", "+");

            var user = await _userManager.FindByEmailAsync(confirm.Email);
            if (user == null) { return NotFound(); }

            var TokenCode = user.EmailToken;
            DateTime emailTokenExpiry = (DateTime)user.EmailConfirmExpiry;
            if (TokenCode != confirm.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(
                    new
                    {
                        StatusCode = 400,
                        Message = "NO"
                    });
            }

            //await _userManager.ConfirmEmailAsync(user, newToken);
            user.EmailConfirmed = true;

            
            _authContext.Update(user);
            await _authContext.SaveChangesAsync();
            return Ok();


        }

    }
}
