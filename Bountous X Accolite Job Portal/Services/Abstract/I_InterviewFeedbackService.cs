using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackModels;
using Bountous_X_Accolite_Job_Portal.Models.InterviewFeedbackModels.InterviewFeedbackResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface I_InterviewFeedbackService
    {
        Task<AllInterviewFeedbackResponseViewModel> GetAllInterviewFeedbacksByAEmployee(Guid EmployeeId);
        Task<InterviewFeedbackResponseViewModel> GetInterviewFeedbackById(Guid Id);
        Task<InterviewFeedbackResponseViewModel> AddInterviewFeedback(CreateInterviewFeedbackViewModel interviewFeedback , Guid EmpId);
        Task<InterviewFeedbackResponseViewModel> EditInterviewFeedback(EditInterviewFeedbackViewModel interviewFeedback , Guid Empid);
        Task<InterviewFeedbackResponseViewModel> DeleteInterviewFeedback(Guid Id);
    }
}
