using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobStatusViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobStatusService : IJobStatusService
    {
        private readonly ApplicationDbContext _dbContext;
        public JobStatusService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public int getInitialReferralStatus()
        {
            int statusId = -1;

            List<Status> status = _dbContext.Status.ToList();
            foreach(Status s in status)
            {
                if(String.Equals(s.StatusName.ToLower(), "referred"))
                {
                    statusId = s.StatusId;
                }
            }

            return statusId;
        }

        public int GetInitialStatus()
        {
            return 5;
        }

        public bool IsRejectedStatus(int StatusId)
        {
            var status = _dbContext.Status.Find(StatusId);
            if(status == null)
            {
                return false;
            }

            if (String.Equals(status.StatusName.ToLower(), "rejected"))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> AddStatus(AddJobStatusViewModel jobstatus, Guid empId)
        {
            if (jobstatus == null || jobstatus.StatusName == null)
            {
                return false;
            }
            Status addJobStatus = new Status();
            addJobStatus.StatusName = jobstatus.StatusName;
            addJobStatus.CreatedAt = DateTime.Now;
            addJobStatus.EmpId = empId;

            await _dbContext.Status.AddAsync(addJobStatus);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        
    }
}