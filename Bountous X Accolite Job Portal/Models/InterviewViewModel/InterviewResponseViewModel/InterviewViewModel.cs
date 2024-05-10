namespace Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel.InterviewResponseViewModel
{
    public class InterviewViewModel : ResponseViewModel
    {
        public Guid? InterviewId { get; set; }
        public Guid? ApplicationId { get; set; }
        public Guid? ClosedApplicationId { get; set; }
        public DateOnly? InterviewDate { get; set; }
        public TimeOnly? InterviewTime { get; set; }
        public Guid? InterViewerId { get; set; }
        public string? Link { get; set; }
        public Guid? FeedbackId { get; set; }
        public Guid? EmpId { get; set; }

        public InterviewViewModel(Interview interview)
        {
            InterviewId = interview.InterviewId;
            ApplicationId = interview.ApplicationId;
            ClosedApplicationId = interview.ClosedJobApplicationId;
            InterviewDate = interview.InterviewDate;
            InterviewTime = interview.InterviewTime;
            InterViewerId = interview.InterViewerId;
            Link = interview.Link;
            FeedbackId = interview.FeedbackId;
            EmpId = interview.EmpId;
        }
    }
}
