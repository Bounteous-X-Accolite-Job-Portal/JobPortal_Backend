namespace Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel.ResponseViewModels
{
    public class ReferralViewModel
    {
        public Guid ReferralId { get; set; }
        public Guid? CandidateId { get; set; }
        public Guid? JobId { get; set; }
        public int? StatusId { get; set; }
        public Guid? ClosedJobId { get; set; }
        public Guid? ApplicationId { get; set; }
        public Guid? ClosedApplicationId { get; set; }

        public ReferralViewModel(Referral referral)
        {
            ReferralId = referral.ReferralId;
            CandidateId = referral.CandidateId;
            JobId = referral.JobId;
            StatusId = referral.StatusId;
            ClosedJobId = referral.ClosedJobId;
            ApplicationId = referral.ApplicationId;
            ClosedApplicationId = referral.ClosedApplicationId;
        }
    }
}
