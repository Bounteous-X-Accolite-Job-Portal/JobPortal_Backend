using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackModels.InterviewFeedbackResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel.InterviewResponseViewModel
{
    public class ApplicantInterviewViewModel
    {
        public InterviewViewModel Interview { get; set; }
        public EmployeeViewModels Interviewer { get; set; }
        public InterviewFeedbackViewModel? Feedback { get; set; }
    }
}
