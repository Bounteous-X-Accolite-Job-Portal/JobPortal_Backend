using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class JobCategory
    {
        [Key]
        public Guid CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? EmpId { get; set; }
        public virtual Employee? Employee { get; set; }

     //   public ICollection<JobPosition> JobPositions { get; set; }  
    }
}
