using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackViewModel.InterviewFeedbackResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface I_InterviewFeedbackService
    {
        AllInterviewFeedbackResponseViewModel GetAllInterviewFeedbacks();
        InterviewFeedbackResponseViewModel GetInterviewFeedbackById(Guid Id);
        Task<InterviewFeedbackResponseViewModel> AddInterviewFeedback(CreateInterviewFeedbackViewModel interviewFeedback , Guid EmpId);
        Task<InterviewFeedbackResponseViewModel> EditInterviewFeedback(EditInterviewFeedbackViewModel interviewFeedback , Guid Empid);
        Task<InterviewFeedbackResponseViewModel> DeleteInterviewFeedback(Guid Id);
    }
}
