using Bountous_X_Accolite_Job_Portal.Models.ResumeModels.ResponseViewModels;

namespace Bountous_X_Accolite_Job_Portal.Models.ResumeModels
{
    public class ResumeViewModel
    {
        public Guid ResumeId { get; set; }
        public string ResumeUrl { get; set; }
        public Guid? CandidateId { get; set; }

        public ResumeViewModel(Resume resume)
        {
            ResumeId = resume.ResumeId;
            ResumeUrl = resume.ResumeUrl;
            CandidateId = resume.CandidateId;
        }

        public static implicit operator ResumeViewModel(ResumeResponseViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}
