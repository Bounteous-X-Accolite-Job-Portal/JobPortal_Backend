using System.ComponentModel.DataAnnotations;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Interview
    {
        [Key]
        public Guid InterviewId { get; set; }
        public Guid? ApplicationId { get; set; } 
        public virtual JobApplication? JobApplication { get; set; } 
        public Guid? ClosedJobApplicationId { get; set; }
        public virtual ClosedJobApplication? ClosedJobApplication { get; set; }
        public DateOnly? InterviewDate { get; set; }
        public TimeOnly? InterviewTime { get; set; }
        public Guid? InterViewerId { get; set; }
        public virtual Employee? Interviewer { get; set; }
        public string? Link {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? FeedbackId { get; set; }
        public virtual InterviewFeedback? Feedback { get; set; }
        public Guid? EmpId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
