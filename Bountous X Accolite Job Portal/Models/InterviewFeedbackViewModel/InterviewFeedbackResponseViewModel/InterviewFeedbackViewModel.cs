namespace Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackViewModel.InterviewFeedbackResponseViewModel
{
    public class InterviewFeedbackViewModel : ResponseViewModel
    {
        public int Rating { get; set; }
        public string Feedback { get; set; }
        public string? AdditionalLink { get; set; }
        public Guid? EmployeeId { get; set; }

        public InterviewFeedbackViewModel(InterviewFeedback feedback)
        {
            Rating = feedback.Rating;
            Feedback = feedback.Feedback;
            AdditionalLink = feedback.AdditionalLink;
            EmployeeId = feedback.EmployeeId;
        }
    }
}
