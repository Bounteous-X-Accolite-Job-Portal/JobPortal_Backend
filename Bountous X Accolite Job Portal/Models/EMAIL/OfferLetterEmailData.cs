namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public class OfferLetterEmailData
    {
        public Attachment Attachments { get; set; }

        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        //public IFormFileCollection Attachments { get; set; }


        public OfferLetterEmailData(string email, string to, string body, Attachment attachments)
        {
            this.To = email;
            this.Subject = to;
            this.Body = body;
            this.Attachments = attachments;
        }
    }
}
