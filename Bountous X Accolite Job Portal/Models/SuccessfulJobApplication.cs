using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class SuccessfulJobApplication
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }
        public Guid? ApplicationId { get; set; }
        public virtual JobApplication? JobApplication { get; set; }
    }
}
