namespace Bountous_X_Accolite_Job_Portal.Models.JobApplicationModels
{
    public class SuccessfulApplicationViewModel
    {
        public Guid Id { get; set; }
        public bool IsOfferLetterGenerated { get; set; }
        public Guid? CandidateId { get; set; }
        public Guid? ApplicationId { get; set; }
        public Guid? JobId { get; set; }
        public Guid? ClosedJobId { get; set; }
        public SuccessfulApplicationViewModel(SuccessfulJobApplication application)
        {
            Id = application.Id;
            CandidateId = application.CandidateId;
            ApplicationId = application.ApplicationId;
            IsOfferLetterGenerated = application.IsOfferLetterGenerated;
            JobId = application.JobId;
            ClosedJobId = application.ClosedJobId;
        }
    }
}
