using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel;
using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel.InterviewResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface I_InterviewService
    {
        Task<All_InterviewResponseViewModel> GetAllInterviews();
        Task<All_InterviewResponseViewModel> GetAllInterviewsForInterviewer(Guid InterViewerId);
        Task<InterviewResponseViewModel> GetInterviewById(Guid Id);
        Task<InterviewResponseViewModel> DeleteInterview(Guid Id);
        Task<InterviewResponseViewModel> AddInterview(CreateInterviewViewModel interview , Guid EmpId);
        Task<InterviewResponseViewModel> EditInterview(EditInterviewViewModel interview);
        Task<bool> UpdateFeedbackId(Guid InterviewId, Guid FeedbackId);
        Task<All_InterviewResponseViewModel> GetAllInterviewByApplicationId(Guid ApplicationId);
        Task<AllApplicantInterviewResponseViewModel> GetAllApplicantInterviewByApplicantionId(Guid ApplicationId);
    }
}
