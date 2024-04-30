namespace Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel
{
    public class CreateInterviewViewModel
    {
        public CreateInterviewViewModel() {}
        public Guid? ApplicationId { get; set; }
        public DateOnly? InterviewDate { get; set; }
        public TimeOnly? InterviewTime { get; set; }
        public Guid? InterViewerId { get; set; }
        public string? Link { get; set; }

    }
}
