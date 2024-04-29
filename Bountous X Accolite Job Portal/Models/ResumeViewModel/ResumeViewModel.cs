namespace Bountous_X_Accolite_Job_Portal.Models.ResumeViewModel
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
    }
}
