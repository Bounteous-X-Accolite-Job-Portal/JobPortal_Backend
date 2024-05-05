using static System.Net.Mime.MediaTypeNames;

namespace Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel
{
    public class JobApplicationViewModel
    {
        public Guid ApplicationId { get; set; }
        public Guid? CandidateId { get; set; }

        public DateTime AppliedOn { get; set; } = DateTime.Now;
  
        public int? StatusId { get;  set; }
        public Guid? JobId { get; set; }

        public JobApplicationViewModel(JobApplication application)
        {
            ApplicationId = application.ApplicationId;
            CandidateId = application.CandidateId;
            AppliedOn = application.AppliedOn;
            StatusId = application.StatusId;
            JobId = application.JobId;
        }

        public JobApplicationViewModel(ClosedJobApplication closedApplication)
        {
            ApplicationId = closedApplication.ApplicationId;
            CandidateId = closedApplication.CandidateId;
            AppliedOn = closedApplication.AppliedOn;
            StatusId = closedApplication.StatusId;
            JobId = closedApplication.JobId;
        }
    }
}

