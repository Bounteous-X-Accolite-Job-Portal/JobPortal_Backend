namespace Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel
{
    public class JobApplicationViewModel
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
  
        public DateTime AppliedOn { get; set; }
  
        public int StatusId { get;  set; }
        public Guid JobId { get; set; }
        public string ApplicationStatus { get; set; }

        
    }
}

