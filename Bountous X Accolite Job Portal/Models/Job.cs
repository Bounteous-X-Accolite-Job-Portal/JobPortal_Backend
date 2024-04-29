using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Job
    {
        [Key]
        public Guid JobId { get; set; }
        public string JobCode { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; } 
        public DateTime PostDate { get; set; } = DateTime.Now;
        public DateTime LastDate { get; set; }
        public Guid DegreeId { get; set; }
        public virtual Degree Degree { get; set; }
        public string Experience { get; set; }
        public Guid CategoryId { get; set; }
        public virtual JobCategory jobCategory { get; set; }
        public Guid PositionId { get; set; }
        public virtual JobPosition jobPosition { get; set; }
        public string JobType { get; set; }
        public Guid LocationId { get; set; }
        public virtual JobLocation jobLocation { get; set; }
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

    }
}
