using Bountous_X_Accolite_Job_Portal.Models.EMAIL;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using MimeKit;
using iText.Kernel.Pdf;
using iText.Forms;
using iText.Forms.Fields;


namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class OfferLetterEmailService : IOfferLetterEmailService
    {
        private readonly IConfiguration _config;

        public OfferLetterEmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailData request, string name)
        {
            var email = new MimeMessage();
            var from = _config["EmailSettings:From"];
            email.From.Add(new MailboxAddress("Job Portal", from));
            email.To.Add(new MailboxAddress(request.To, request.To));
            email.Subject = request.Subject;

            string emailBody = OfferLetterEmailBody.EmailStringBody(name);

            var multipart = new Multipart("mixed");

            var htmlPart = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailBody
            };

            multipart.Add(htmlPart);

            var attachment = new MimePart()
            {
                Content = new MimeContent(System.IO.File.OpenRead("NewCandidateofferletter.pdf")),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = name + "_offerletter.pdf"
            };

            multipart.Add(attachment);

            email.Body = multipart;

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            {
                try
                {
                    smtp.Connect(_config["EmailSettings:SmtpServer"], 465, true);
                    smtp.Authenticate(_config["EmailSettings:From"], _config["EmailSettings:EmailPassword"]);
                    smtp.Send(email);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error sending offer letter email", ex);
                }
                finally
                {
                    smtp.Disconnect(true);
                }
            }
        }
    }
}

