
using Microsoft.AspNetCore.Mvc;
using MimeKit;

using MailKit.Net.Smtp;
using MailKit.Security;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;



namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmail(EmailData request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }
    }
    //    [HttpPost]
    //    [Route("EMail")]
    //    public IActionResult SendEmail(string body)
    //    {
    //        var email = new MimeMessage();
    //        email.From.Add(MailboxAddress.Parse("euna.beatty55@ethereal.email"));
    //        email.To.Add(MailboxAddress.Parse("shagunpsit@gmail.com"));
    //        email.Subject = "HELLL";
    //        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

    //        using var smtp=new SmtpClient();
    //        smtp.Connect("smtp.ethereal.email",587,SecureSocketOptions.StartTls);
    //        smtp.Authenticate("euna.beatty55@ethereal.email", "Wz1BrHgkvF1fwxqJ35");
    //        smtp.Send(email);
    //        smtp.Disconnect(true);

    //        return Ok();
    //    }
    //}
}
