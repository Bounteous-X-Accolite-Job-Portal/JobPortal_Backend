using Bountous_X_Accolite_Job_Portal.Models.EMAIL;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using MimeKit;
using Aspose;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using Aspose.Pdf;
using Aspose.Pdf.Text;


namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class OfferLetterEmailService : IOfferLetterEmailService
    {
        private readonly IConfiguration _config;

        public OfferLetterEmailService(IConfiguration config)
        {
            _config = config;
        }

        public void AddNameToLetter(string name)
        {
            Document pdfDoc = new Document("offerletter.pdf");

            TextFragmentAbsorber absorber = new TextFragmentAbsorber("<<NAME>>");

            pdfDoc.Pages.Accept(absorber);

            TextFragmentCollection textFragments = absorber.TextFragments;

            foreach (var textFragment in textFragments)
            {
                textFragment.Text = name;
                break;
            }

            pdfDoc.Save(name + "_offerletter.pdf");
        }

        public void SendEmail(EmailData request, string name)
        {
            AddNameToLetter(name);

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
                Content = new MimeContent(System.IO.File.OpenRead(name + "_offerletter.pdf")),
                ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = name + "-offerletter.pdf"
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

