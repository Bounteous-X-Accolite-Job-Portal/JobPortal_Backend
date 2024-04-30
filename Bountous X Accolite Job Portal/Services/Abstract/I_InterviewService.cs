using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel.InterviewResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface I_InterviewService
    {
        All_InterviewResponseViewModel GetAllInterviews();
       // All_InterviewResponseViewModel GetInterviewsForCandidate(Guid Ca);
        InterviewResponseViewModel GetInterviewById(Guid Id);
        Task<InterviewResponseViewModel> DeleteInterview(Guid Id);
        Task<InterviewResponseViewModel> AddInterview(CreateInterviewViewModel interview , Guid EmpId);
        Task<InterviewResponseViewModel> EditInterview(EditInterviewViewModel interview);
    }
}
