namespace Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel
{
    public class SuccessfulApplicationViewModel
    {
        public Guid ApplicationId { get; set; }
        public Guid? CandidateId { get; set; }
        public Guid? JobId { get; set; }
        
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
