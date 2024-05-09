using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class ClosedJob
    {
        [Key]
        public Guid ClosedJobId { get; set; }
        public Guid JobId { get; set; }
        public string JobCode { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public DateTime PostDate { get; set; } = DateTime.Now;
        public DateTime LastDate { get; set; }
        public Guid? DegreeId { get; set; }
        public virtual Degree? Degree { get; set; }
        public string Experience { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual JobCategory? jobCategory { get; set; }
        public Guid? PositionId { get; set; }
        public virtual JobPosition? jobPosition { get; set; }
        public Guid? JobTypeId { get; set; }
        public virtual JobType? jobType { get; set; }
        public Guid? LocationId { get; set; }
        public virtual JobLocation? jobLocation { get; set; }
        public Guid? EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }

        public ClosedJob()
        {
            
        }

        public ClosedJob(Job job)
        {
            JobId = job.JobId;
            JobCode = job.JobCode;
            JobTitle = job.JobTitle;
            JobDescription = job.JobDescription;
            PostDate = job.PostDate;
            LastDate = job.LastDate;
            DegreeId = job.DegreeId;
            Experience = job.Experience;
            CategoryId = job.CategoryId;
            PositionId = job.PositionId;
            JobTypeId = job.JobTypeId;
            LocationId = job.LocationId;
            EmployeeId = job.EmployeeId;
        }
    }
}
