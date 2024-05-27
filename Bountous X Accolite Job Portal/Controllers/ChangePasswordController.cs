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
        private readonly SignInManager<User>   _signInManager;
        public ChangePasswordController(IEmailService emailService, IConfiguration config, ApplicationDbContext applicationDbContext, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _emailService = emailService;
            _config = config;
            _authContext = applicationDbContext;
            _userManager = userManager;
            _signInManager = signInManager;

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

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<ResponseViewModel> ChangePassword(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            ResponseViewModel response = new ResponseViewModel();

            if (user == null) 
            {
                response.Status = 404;
                response.Message = "User Not Found !!";

                return response;
            }

            var TokenCode = await _userManager.GeneratePasswordResetTokenAsync(user);
   
            var oldPassword = await _signInManager.CheckPasswordSignInAsync(user, resetPassword.CurrentPassword, lockoutOnFailure: false);
            if (oldPassword.Succeeded == true)
            {
                var result = await _userManager.ResetPasswordAsync(user, TokenCode, resetPassword.NewPassword);
                _authContext.Entry(user).State = EntityState.Modified;
                await _authContext.SaveChangesAsync();


                response.Status = 200;
                response.Message = "Password Changed Successfully !!";
            }
            else
            {
                response.Status = 403;
                response.Message = "Current Password is not valid!!";
            }

            return response;
        }

    }
}
