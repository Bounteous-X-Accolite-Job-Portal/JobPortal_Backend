using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Referral
    {
        [Key]
        public Guid ReferralId { get; set; }
        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }
        public Guid? JobId { get; set; }
        public virtual Job? Job { get; set; }
        public DateTime ReferredOn { get; set; } = DateTime.Now;
        public int? StatusId { get; set; }
        public virtual Status? Status { get; set; }
        public Guid? EmpId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
