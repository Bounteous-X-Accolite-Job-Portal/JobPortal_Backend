using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class InterviewFeedback
    {
        [Key]
        public Guid FeedbackId { get; set; }
        public Guid InterviewId { get; set; }
        public virtual Interview Interview { get; set; }
        public int Rating { get; set; }
        public string Feedback {  get; set; }
        public string? AdditionalLink { get; set; } 
    }
}
