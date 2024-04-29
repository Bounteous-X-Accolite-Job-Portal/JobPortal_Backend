using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Bountous_X_Accolite_Job_Portal.Models;
using Microsoft.AspNetCore.Identity;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; }
        public string EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public int DesignationId { get; set; }
        public Designation Designation { get; set; }
    }
}
