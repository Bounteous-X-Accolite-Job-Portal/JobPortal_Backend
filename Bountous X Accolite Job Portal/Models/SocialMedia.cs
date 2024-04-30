using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class SocialMedia
    {
        [Key]
        public Guid SocialMediaId { get; set; }
        public string Link1 { get; set; }
        public string Link2 { get; set; }
        public string Link3 { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }
    }
}
