using Bountous_X_Accolite_Job_Portal.Models.SkillsModels;
using Bountous_X_Accolite_Job_Portal.Models.SkillsModels.ResponseViewModels;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface ISkillsService
    {
        SkillsResponseViewModel GetSkillsOfACandidate(Guid CandidateId);
        SkillsResponseViewModel GetSkillsById(Guid Id);
        Task<SkillsResponseViewModel> AddSkills(AddSkillsViewModel addSkills, Guid CandidateId);
        Task<SkillsResponseViewModel> UpdateSkills(UpdateSkillsViewModel updateSkills);
        Task<SkillsResponseViewModel> RemoveSkills(Guid Id);
    }
}
