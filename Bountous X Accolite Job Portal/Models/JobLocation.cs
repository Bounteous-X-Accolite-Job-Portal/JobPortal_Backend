using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class JobLocation
    {
        [Key]
        public Guid LocationId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? EmpId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
