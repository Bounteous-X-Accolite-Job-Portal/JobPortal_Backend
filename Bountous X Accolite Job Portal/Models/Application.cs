using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Application
    {
        [Key]
        public Guid ApplicationId { get; set; }
        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }
        public Guid? JobId { get; set; }
        public virtual Job? Job { get; set; }
        public DateTime AppliedOn { get; set; } = DateTime.Now;


    }
}
