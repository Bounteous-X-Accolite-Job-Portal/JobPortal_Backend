﻿
using System.Security.Cryptography;
using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.EMAIL;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ApplicationDbContext _authContext;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IOfferLetterEmailService _offerLetterEmailService;
        public EmailController(IEmailService emailService, IConfiguration config, ApplicationDbContext applicationDbContext, UserManager<User> userManager, IOfferLetterEmailService offerLetterEmailService)
        {
            _emailService = emailService;
            _config = config;
            _authContext = applicationDbContext;
            _userManager = userManager;
            _offerLetterEmailService = offerLetterEmailService;
        }

        [HttpPost("Email/{email}")]

        public async Task<IActionResult> SendEmail(string email, IConfiguration _config)
        {
            var user = await _userManager.FindByEmailAsync(email);

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
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(5);

            string from = _config["EmailSettings:From"];
            var emailModel = new EmailData(email, "ResetPassword", EmailBody.EmailStringBody(user.UserName, email, emailToken));
            //Debug.WriteLine("INSIDE CONTROLLER", emailModel);

            _emailService.SendEmail(emailModel);

         //   EmailData emailModel = new EmailData(email, "Congratulations! Offer of Employment with bounteous x Accolite",
            //        OfferLetterEmailBody.EmailStringBody("Ankit"));


         //   _offerLetterEmailService.SendEmail(emailModel, "Ankit");

            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();
            return Ok();

        }
        [HttpPost("reset-Password")]
        public async Task<ResponseViewModel> ResetPassword(ResetPasswordDTO resetPassword)
        {
            ResponseViewModel response = new ResponseViewModel();

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                response.Status = 403;
                response.Message = "User Not Exist !!";
                return response;
            }

            var TokenCode = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, TokenCode, resetPassword.NewPassword);

            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Password Reset Successfully !!";

            return response;

        }

    }
}

