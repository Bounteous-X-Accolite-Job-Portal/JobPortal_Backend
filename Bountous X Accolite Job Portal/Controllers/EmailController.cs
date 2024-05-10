
using Microsoft.AspNetCore.Mvc;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using System.Security.Cryptography;
using MailKit.Net.Smtp;
using Bountous_X_Accolite_Job_Portal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bountous_X_Accolite_Job_Portal.Models.EMAIL;

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
        public EmailController(IEmailService emailService,IConfiguration config,ApplicationDbContext applicationDbContext,UserManager<User> userManager)
        {
            _emailService = emailService;
            _config = config;
            _authContext = applicationDbContext;
            _userManager = userManager;
            
        }

        [HttpPost("Email/{email}")]
        
        public async Task<IActionResult> SendEmail(string email,IConfiguration _config)
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
            var emailModel = new EmailData(email, "ResetPassword", EmailBody.EmailStringBody(email, emailToken));
            //Debug.WriteLine("INSIDE CONTROLLER", emailModel);

            _emailService.SendEmail(emailModel);

            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();
            return Ok();
            
        }
        [HttpPost("reset-Password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPassword)
        {
           

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null) { return NotFound(); }

            var TokenCode = await _userManager.GeneratePasswordResetTokenAsync(user);
            DateTime emailTokenExpiry= (DateTime)user.ResetPasswordExpiry;
            if(emailTokenExpiry<DateTime.Now)
            {
                return BadRequest(
                    new
                    {
                        StatusCode = 400,
                        Message = "NO"
                    });
            }
            var result = await _userManager.ResetPasswordAsync(user, TokenCode, resetPassword.NewPassword);

            //String hashedNewPassword = _userManager.PasswordHasher.HashPassword(user, resetPassword.NewPassword);

            //user.PasswordHash = hashedNewPassword;
            _authContext.Entry(user).State=EntityState.Modified;
            await _authContext.SaveChangesAsync();
            //_authContext.Update(user);
            //await _authContext.SaveChangesAsync();
            return Ok();

            }

        
    }
    }
    
   
         //var email = new MimeMessage();
        //email.From.Add(MailboxAddress.Parse("euna.beatty55@ethereal.email"));
        //email.To.Add(MailboxAddress.Parse("shagunpsit@gmail.com"));
        //email.Subject = "HELLL";
        //email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

        //using var smtp = new SmtpClient();
        //smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
        //smtp.Authenticate("euna.beatty55@ethereal.email", "Wz1BrHgkvF1fwxqJ35");
        //smtp.Send(email);
        //smtp.Disconnect(true);

        //return Ok();
    


