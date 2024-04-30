using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Skills
    {
        [Key]
        public Guid SkillsId { get; set; }
        public string CandidateSkills { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }
    }
}
