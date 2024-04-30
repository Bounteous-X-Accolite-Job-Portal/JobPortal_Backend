using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel.ResponseViewModels;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface ICandidateEducationService
    {
        MultipleEducationResponseViewModel GetAllEducationOfACandidate(Guid CandidateId);
        CandidateEducationResponseViewModel GetEducationById(Guid Id);
        Task<CandidateEducationResponseViewModel> AddCandidateEducation(AddCandidateEducationViewModel addCandidateEducation, Guid CandidateId);
        Task<CandidateEducationResponseViewModel> UpdateCandidateEducation(UpdateCandidateEducationViewModel updateEducation);
        Task<CandidateEducationResponseViewModel> RemoveEducation(Guid Id);
    }
}
