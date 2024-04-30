namespace Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel.InterviewResponseViewModel
{
    public class DisplayInterviewResponseViewModel : ResponseViewModel
    {
        public Guid? ApplicationId { get; set; }
        public DateOnly? InterviewDate { get; set; }
        public TimeOnly? InterviewTime { get; set; }
        public Guid? InterViewerId { get; set; }
        public string? Link { get; set; }

        public DisplayInterviewResponseViewModel(){}
        public DisplayInterviewResponseViewModel(Interview interview)
        {
            ApplicationId = interview.ApplicationId;
            InterviewDate = interview.InterviewDate;
            InterviewTime = interview.InterviewTime;
            InterViewerId = interview.InterViewerId;
            Link = interview.Link;
        }
    }
}
