using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class CandidateExperience
    {
        [Key]
        public Guid ExperienceId { get; set; }
        public string ExperienceTitle { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsCurrentlyWorking { get; set; }
        public string Description { get; set; }
        public Guid? CompanyId { get; set; }
        public virtual Company? Company { get; set; }    
        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }    
    }
}
