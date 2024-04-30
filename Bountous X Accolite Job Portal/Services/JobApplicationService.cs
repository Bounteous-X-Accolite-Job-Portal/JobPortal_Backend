using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Microsoft.AspNetCore.Identity;
using static System.Net.Mime.MediaTypeNames;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobApplicationService : IApplicationService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public JobApplicationService(UserManager<User> userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _dbContext = applicationDbContext;
        }

        public async Task<bool> AddApplications(JobApplicationViewModel application)
        {
            if (application == null || application.CandidateId == null)
            {
                return false;
            }

            JobApplication jobApplication=new JobApplication();
            jobApplication.ApplicationId = application.ApplicationId;
            jobApplication.CandidateId = application.CandidateId;
            jobApplication.StatusId = application.StatusId;
            jobApplication.AppliedOn = application.AppliedOn;
            jobApplication.ApplicationStatus= application.ApplicationStatus;
            jobApplication.JobId= application.JobId;

            await _dbContext.JobApplications.AddAsync(jobApplication);
            await _dbContext.SaveChangesAsync();

            return true;



        }
    }

}
