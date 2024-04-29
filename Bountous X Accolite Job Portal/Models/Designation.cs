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
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        //[ForeignKey(nameof(DesignationId))]
        public Guid? EmpId { get; set; }
        public Employee? Employee { get; set; }
    }
}
