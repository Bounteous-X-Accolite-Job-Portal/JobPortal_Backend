using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class JobApplication
    {
        [Key]
        public Guid ApplicationId { get; set; }
        public Guid? CandidateId { get; set; }
        public Candidate? Candidate { get; set; }   

        public Guid? JobId { get; set; }
        public Job? Job { get; set; }    

        public DateTime AppliedOn { get; set; }

        public string ApplicationStatus { get; set; }
        public int? StatusId { get; set; }
        public Status? Status { get; set; }



    }
}
