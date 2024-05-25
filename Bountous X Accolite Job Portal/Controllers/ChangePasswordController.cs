using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.ChangePasswordModels;
using Bountous_X_Accolite_Job_Portal.Models.EMAIL;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePasswordController : Controller
    {

        private readonly ApplicationDbContext _authContext;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        public ChangePasswordController(IEmailService emailService, IConfiguration config, ApplicationDbContext applicationDbContext, UserManager<User> userManager)
        {
            _emailService = emailService;
            _config = config;
            _authContext = applicationDbContext;
            _userManager = userManager;

        }
        [HttpPost("ChangePasswordEmail/{email}")]
        public async Task<IActionResult> SendEmail(string email, IConfiguration _config)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var changePasswordToken = Convert.ToBase64String(tokenBytes);

            user.ChangePasswordToken = changePasswordToken;
            user.ChangePasswordExpiry= DateTime.Now.AddMinutes(5);

            string from = _config["EmailSettings:From"];
            var changePasswordModel = new EmailData(email, "ResetPassword", ChangePasswordBody.EmailStringBody(email, changePasswordToken));

            _emailService.SendEmail(changePasswordModel);

            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();
            return Ok();


        }

        [HttpPost("ChangePasswordEmail")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePassword)
        {
            var user = await _userManager.FindByEmailAsync(changePassword.Email);
            if (user == null) { return NotFound(); }

            var TokenCode = await _userManager.GeneratePasswordResetTokenAsync(user);
            DateTime emailTokenExpiry = (DateTime)user.ChangePasswordExpiry;
            if (emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(
                    new
                    {
                        StatusCode = 400,
                        Message = "NO"
                    });
            }
            var result = await _userManager.ResetPasswordAsync(user, TokenCode, changePassword.NewPassword);
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();
            return Ok();
        }

    }
}
