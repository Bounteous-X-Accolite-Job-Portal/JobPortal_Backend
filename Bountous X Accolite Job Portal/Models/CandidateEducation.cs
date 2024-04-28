using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class CandidateEducation
    {
        [Key]
        public Guid EducationId { get; set; }
        public string? InstitutionOrSchoolName { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public double Grade { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? InstitutionId { get; set; }
        public virtual EducationInstitution? EducationInstitution { get; set; }
        public Guid? DegreeId { get; set; }
        public virtual Degree? Degree { get; set; }  
        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }
    }
}
