using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels;
using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobStatusViewModel;
using Bountous_X_Accolite_Job_Portal.Models.ResumeModels;
using Bountous_X_Accolite_Job_Portal.Models.SkillsModels;


namespace Bountous_X_Accolite_Job_Portal.Models.JobApplicationModels.ResponseViewModels
{
    public class ApplicantViewModel
    {
        public Guid ApplicationId { get; set; }
        public Guid JobId { get; set; }
        public Guid CandidateId { get; set; }
        public DateTime AppliedOn { get; set; }
        public CandidateViewModel Candidate { get; set; }
        public SkillsViewModel? Skills { get; set; }
        public ResumeViewModel? Resume { get; set; }
        public StatusViewModel? Status { get; set; }
        public List<ExperienceWithCompanyViewModel> Experience { get; set; }
        public List<CompleteEducationViewModel> Education {  get; set; }
    }
}
