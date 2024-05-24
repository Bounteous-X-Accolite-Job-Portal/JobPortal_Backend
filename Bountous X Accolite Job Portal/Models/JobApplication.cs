using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class JobApplication
    {

        [Key]
        public Guid ApplicationId { get; set; }
        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }   
        public Guid? JobId { get; set; }
        public virtual Job? Job { get; set; }
        public Guid? ClosedJobId { get; set; }
        public virtual ClosedJob? ClosedJob { get; set; }
        public DateTime AppliedOn { get; set; } = DateTime.Now;
        public int? StatusId { get; set; }
        public virtual Status? Status { get; set; }
    }
}
