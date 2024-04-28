using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel;
using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel.ResponseViewModels;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface ICandidateExperienceService
    {
        MultipleExperienceResponseViewModel GetAllExperienceOfACandidate(Guid CandidateId);
        CandidateExperienceResponseViewModel GetExperienceById(Guid Id);
        Task<CandidateExperienceResponseViewModel> AddCandidateExperience(AddCandidateExperienceViewModel addExperience, Guid CandidateId);
        Task<CandidateExperienceResponseViewModel> UpdateCandidateExperience(UpdateCandidateExperienceViewModel updateExperience);
        Task<CandidateExperienceResponseViewModel> RemoveExperience(Guid Id);
    }
}
