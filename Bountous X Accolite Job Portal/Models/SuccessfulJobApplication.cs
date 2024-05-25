using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class SuccessfulJobApplication
    {
        [Key]
        public Guid Id { get; set; }
        public bool IsOfferLetterGenerated { get; set; } = false;
        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }
        public Guid? ApplicationId { get; set; }
        public virtual JobApplication? JobApplication { get; set; }
        public Guid? JobId { get; set; }
        public virtual Job? Job { get; set; }
        public Guid? ClosedJobId { get; set; }
        public virtual ClosedJob? ClosedJob { get; set; }
    }
}
