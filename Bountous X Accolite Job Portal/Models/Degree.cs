using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Degree
    {
        [Key]
        public Guid DegreeId { get; set; }
        public string DegreeName { get; set; }
        public int DurationInYears { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? EmpId { get; set; } 
        public virtual Employee? Employee { get; set; }  
    }
}
