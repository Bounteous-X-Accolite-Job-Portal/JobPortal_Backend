using System.ComponentModel.DataAnnotations;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackViewModel;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class InterviewFeedback
    {
        [Key]
        public Guid? FeedbackId { get; set; }
        public Guid? InterviewId { get; set; }
        public virtual Interview? Interview { get; set; }
        public int Rating { get; set; }
        public string Feedback {  get; set; }
        public string? AdditionalLink { get; set; }

        public InterviewFeedback()
        {
        }

        public InterviewFeedback(CreateInterviewFeedbackViewModel feedback)
        {
            InterviewId = feedback.InterviewId;
            Rating = feedback.Rating;
            Feedback = feedback.Feedback;
            AdditionalLink = feedback.AdditionalLink;
        }
    }
}
