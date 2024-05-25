using Bountous_X_Accolite_Job_Portal.Models.EMAIL;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IEmailService
    {
        void SendEmail( EmailData request);
    }
}
