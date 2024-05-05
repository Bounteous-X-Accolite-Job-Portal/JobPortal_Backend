using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobStatusViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobStatusService : IJobStatusService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public int GetInitialStatus()
        {
            return 1;
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

        public JobStatusService(UserManager<User> userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _dbContext = applicationDbContext;
        }
        public async Task<bool> AddStatus(AddJobStatusViewModel jobstatus, Guid empId)
        {
            if (jobstatus == null || jobstatus.StatusName == null)
            {
                return false;
            }
            Status addJobStatus = new Status();
            addJobStatus.StatusName = addJobStatus.StatusName;
            addJobStatus.CreatedAt = DateTime.Now;
            addJobStatus.EmpId = empId;

            await _dbContext.Status.AddAsync(addJobStatus);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        
    }
}