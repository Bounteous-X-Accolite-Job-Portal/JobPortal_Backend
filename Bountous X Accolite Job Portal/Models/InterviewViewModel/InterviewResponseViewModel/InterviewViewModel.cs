namespace Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel.InterviewResponseViewModel
{
    public class InterviewViewModel : ResponseViewModel
    {
        public Guid? ApplicationId { get; set; }
        public DateOnly? InterviewDate { get; set; }
        public TimeOnly? InterviewTime { get; set; }
        public Guid? InterViewerId { get; set; }
        public string? Link { get; set; }

        public InterviewViewModel(){}
        public InterviewViewModel(Interview interview)
        {
            ApplicationId = interview.ApplicationId;
            InterviewDate = interview.InterviewDate;
            InterviewTime = interview.InterviewTime;
            InterViewerId = interview.InterViewerId;
            Link = interview.Link;
        }
    }
}
