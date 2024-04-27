using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class EducationInstitution
    {
        [Key]
        public Guid InstitutionId { get; set; }
        public string InstitutionOrSchool { get; set; }
        public string UniversityOrBoard { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? EmpId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
