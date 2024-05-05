using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

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

        public JobApplicationResponseViewModel GetJobApplicaionById(Guid Id)
        {
            JobApplicationResponseViewModel response = new JobApplicationResponseViewModel();

            var application = _dbContext.JobApplications.Find(Id);
            if(application == null)
            {
                response.Status = 404;
                response.Message = "Application with this Id does not exist";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved job application.";
            response.Application = new JobApplicationViewModel(application);
            return response;
        }

        public AllJobApplicationResponseViewModel GetJobApplicationByCandidateId(Guid CandidateId)
        {
            AllJobApplicationResponseViewModel response = new AllJobApplicationResponseViewModel();

            var candidate = _dbContext.Candidates.Find(CandidateId);
            if(candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            List<JobApplication> applications = _dbContext.JobApplications.Where(item => item.CandidateId == CandidateId).ToList();

            List<JobApplicationViewModel> returnApplications = new List<JobApplicationViewModel>();
            foreach(var application in applications)
            {
                returnApplications.Add(new JobApplicationViewModel(application));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all job applications with this candidateId.";
            response.AllJobApplications = returnApplications;
            return response;
        }

        public AllJobApplicationResponseViewModel GetJobApplicationByJobId(Guid JobId)
        {
            AllJobApplicationResponseViewModel response = new AllJobApplicationResponseViewModel();

            var job = _dbContext.Jobs.Find(JobId);
            if (job == null)
            {
                response.Status = 404;
                response.Message = "Job with this Id does not exist";
                return response;
            }

            List<JobApplication> applications = _dbContext.JobApplications.Where(item => item.JobId == JobId).ToList();

            List<JobApplicationViewModel> returnApplications = new List<JobApplicationViewModel>();
            foreach (var application in applications)
            {
                returnApplications.Add(new JobApplicationViewModel(application));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all job applications with this candidateId.";
            response.AllJobApplications = returnApplications;
            return response;
        }

        public AllJobApplicationResponseViewModel GetClosedJobApplicationByCandidateId(Guid CandidateId)
        {
            AllJobApplicationResponseViewModel response = new AllJobApplicationResponseViewModel();

            var candidate = _dbContext.Candidates.Find(CandidateId);
            if (candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            List<ClosedJobApplication> applications = _dbContext.ClosedJobApplications.Where(item => item.CandidateId == CandidateId).ToList();

            List<JobApplicationViewModel> returnApplications = new List<JobApplicationViewModel>();
            foreach (var application in applications)
            {
                returnApplications.Add(new JobApplicationViewModel(application));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all job applications with this candidateId.";
            response.AllJobApplications = returnApplications;
            return response;
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
       
        public async Task<JobApplicationResponseViewModel> ChangeJobApplicationStatus(Guid ApplicationId, int StatusId)
        {
            JobApplicationResponseViewModel response;

            var application = _dbContext.JobApplications.Find(ApplicationId);
            if(application == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 404;
                response.Message = "Application with this Id does not exist.";
                return response;
            }

            var status = _dbContext.Status.Find(StatusId);
            if(status == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 404;
                response.Message = "Status with this Id does not exist.";
                return response;
            }

            response = new JobApplicationResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully changed the status of application.";

            if (_jobStatusService.IsRejectedStatus(StatusId))
            {
                ClosedJobApplication closedApplication = new ClosedJobApplication(application);
                closedApplication.StatusId = StatusId;

                await _dbContext.ClosedJobApplications.AddAsync(closedApplication);
                _dbContext.JobApplications.Remove(application);

                if(closedApplication == null)
                {
                    response = new JobApplicationResponseViewModel();
                    response.Status = 500;
                    response.Message = "Unable to update status, please try again.";
                    return response;
                }

                response.Application = new JobApplicationViewModel(closedApplication);
            }
            else 
            {
                application.StatusId = StatusId;
                _dbContext.JobApplications.Update(application);

                response.Application = new JobApplicationViewModel(application);
            }

            await _dbContext.SaveChangesAsync();
            return response;
        }

    }
}

