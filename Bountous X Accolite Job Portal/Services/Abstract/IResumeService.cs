using Bountous_X_Accolite_Job_Portal.Models.ResumeModels;
using Bountous_X_Accolite_Job_Portal.Models.ResumeModels.ResponseViewModels;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IResumeService
    {
        ResumeResponseViewModel GetResumeOfACandidate(Guid CandidateId);
        ResumeResponseViewModel GetResumeById(Guid Id);
        Task<ResumeResponseViewModel> AddResume(AddResumeViewModel addResume, Guid CandidateId);
        Task<ResumeResponseViewModel> RemoveResume(Guid Id);
    }
}
