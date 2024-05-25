using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class ClosedJobApplication
    {
        [Key]
        public Guid ClosedJobApplicationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }

        public Guid? JobId { get; set; }
        public virtual Job? Job { get; set; }
        public Guid? ClosedJobId { get; set; }
        public virtual ClosedJob? ClosedJob { get; set; }
        public DateTime AppliedOn { get; set; }

        public int? StatusId { get; set; }
        public virtual Status? Status { get; set; }

        public ClosedJobApplication()
        {
            
        }

        public ClosedJobApplication(JobApplication application)
        {
            ApplicationId = application.ApplicationId;
            CandidateId = application.CandidateId;
            JobId = application.JobId;
            AppliedOn = application.AppliedOn;
            StatusId = application.StatusId;
            ClosedJobId = application.ClosedJobId;
        }
    }
}
