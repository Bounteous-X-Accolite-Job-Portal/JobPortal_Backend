using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class JobPosition
    {
        [Key]
        public Guid PositionId { get; set; }
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? CategoryId { get; set; }
        public virtual JobCategory? JobCategory { get; set; }
        public Guid? EmpId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
