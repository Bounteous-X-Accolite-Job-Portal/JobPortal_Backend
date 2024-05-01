using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobStatusService : IJobStatusService
    {
        public int GetInitialStatus()
        {
            return 1;
        }
    }
}
