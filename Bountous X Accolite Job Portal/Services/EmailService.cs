using MailKit.Net.Smtp;
using MimeKit;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Bountous_X_Accolite_Job_Portal.Models.EMAIL;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class EmailService:IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailData request)
        {
            var email = new MimeMessage();
            var from = _config["EmailSettings:From"];
            email.From.Add(new MailboxAddress("Job Portal",from));
            email.To.Add(new MailboxAddress(request.To,request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            { Text = string.Format(request.Body) };

            using var smtp = new SmtpClient();
            {
                try
                {
                    smtp.Connect(_config["EmailSettings:SmtpServer"], 465, true);
                    var temp = _config.GetSection("EmailSettings:From").Value;
                    var temp2 = _config.GetSection("EmailSettings:EmailPassword").Value;
                    
                    smtp.Authenticate(_config.GetSection("EmailSettings:From").Value, _config.GetSection("EmailSettings:EmailPassword").Value);
                    smtp.Send(email);
                }
                catch (Exception ex)
                {
                    throw;

                }
                finally
                {
                    smtp.Disconnect(true);
                    smtp.Dispose(); 
                }
            }
            //smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            //smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            //smtp.Send(email);
            //smtp.Disconnect(true);
        }
    
}
}
