using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Designation
    {
        [Key]
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? EmpId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
