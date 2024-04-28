using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Resume
    {
        [Key]
        public Guid ResumeId { get; set; }
        public string ResumeUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }
    }
}
