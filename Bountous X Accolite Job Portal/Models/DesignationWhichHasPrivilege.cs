using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class DesignationWhichHasPrivilege
    {
        [Key]
        public int PrivilegeId { get; set; }   
        public int? DesignationId { get; set; }
        public virtual Designation? Designation { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public Guid? EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
