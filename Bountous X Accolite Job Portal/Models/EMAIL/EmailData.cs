namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public class EmailData
    {

        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailData(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;

        }
    }

}
