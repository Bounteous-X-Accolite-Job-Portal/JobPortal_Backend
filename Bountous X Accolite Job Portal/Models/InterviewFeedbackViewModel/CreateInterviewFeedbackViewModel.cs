namespace Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackViewModel
{
    public class CreateInterviewFeedbackViewModel
    {
        public Guid? InterviewId { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }
        public string? AdditionalLink { get; set; }
    }
}
