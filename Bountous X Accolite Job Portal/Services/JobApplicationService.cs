using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IJobStatusService _jobStatusService;
        public JobApplicationService(ApplicationDbContext applicationDbContext, IJobStatusService jobStatusService)
        {
            _dbContext = applicationDbContext;
            _jobStatusService = jobStatusService;
        }

        public async Task<JobApplicationResponseViewModel> Apply(AddJobApplication application, Guid CandidateId)
        {
            JobApplicationResponseViewModel response;

            var candidate = _dbContext.Candidates.Find(CandidateId);
            if(candidate == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist.";
                return response;
            }
           
            var job = _dbContext.Jobs.Find(application.JobId);
            if(job == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 404;
                response.Message = "Job with this id doesn't exist";
                return response;
            }

            JobApplication jobApplication = new JobApplication();
            {
                jobApplication.JobId = application.JobId;
                jobApplication.CandidateId = CandidateId;
                jobApplication.StatusId = _jobStatusService.GetInitialStatus();
            };

            await _dbContext.JobApplications.AddAsync(jobApplication);
            await _dbContext.SaveChangesAsync();

            if(jobApplication == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 500;
                response.Message = "Unable to apply to this job, please try again.";
                return response;
            }

            response = new JobApplicationResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully applied too the job.";
            response.Application = new JobApplicationViewModel(jobApplication);
            return response;
            
        }
       


    }
}

