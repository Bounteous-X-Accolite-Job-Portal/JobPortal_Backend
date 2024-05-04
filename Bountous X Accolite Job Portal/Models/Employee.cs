using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; }    // System Generated Employee Id
        public string EmpId { get; set; } // Comapny's EmployeeId of the Employee
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime JoinedOn { get; set; } = DateTime.Now;
        public int? DesignationId { get; set; }
        public virtual Designation? Designation { get; set; }
        public bool Inactive { get; set; }
    }
}
