using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Job
    {
        [Key]
        public Guid JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobCategory { get; set; }
        public string JobLocation { get; set; }
        public string JobDescription { get; set; } 
        public string JobType { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime LastDate { get; set; }
        public string qualifiicaton { get; set; }
        public string Experience { get; set; }
        public int EmployeeId { get; set; }
        public Emplyoee EmployeeName { get; set; }
    }
}
