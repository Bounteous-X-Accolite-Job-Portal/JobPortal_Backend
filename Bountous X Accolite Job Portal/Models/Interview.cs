using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Interview
    {
        [Key]
        public Guid InterviewId { get; set; }
        public Guid? ApplicationId { get; set; } // TO be changed after corresponding model creation
        public virtual Application? Application { get; set; } // TO be changed after corresponding model creation
        public DateOnly? InterviewDate { get; set; }
        public TimeOnly? InterviewTime { get; set; }
        public Guid? InterViewerId { get; set; }
        public virtual Employee? Interviewer { get; set; }
        public string? Link {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? EmpId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
