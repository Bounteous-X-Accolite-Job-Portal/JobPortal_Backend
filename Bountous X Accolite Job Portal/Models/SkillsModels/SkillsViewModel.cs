using Bountous_X_Accolite_Job_Portal.Models.SkillsModels.ResponseViewModels;

namespace Bountous_X_Accolite_Job_Portal.Models.SkillsModels
{
    public class SkillsViewModel
    {
        public Guid SkillsId { get; set; }
        public string CandidateSkills { get; set; }
        public Guid? CandidateId { get; set; }

        public SkillsViewModel(Skills skills)
        {
            SkillsId = skills.SkillsId;
            CandidateSkills = skills.CandidateSkills;
            CandidateId = skills.CandidateId;
        }

        public static implicit operator SkillsViewModel(SkillsResponseViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}
