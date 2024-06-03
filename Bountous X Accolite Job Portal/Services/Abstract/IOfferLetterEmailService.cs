using Bountous_X_Accolite_Job_Portal.Models.EMAIL;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IOfferLetterEmailService
    {
        void SendEmail(EmailData request , string name);
    }
}
