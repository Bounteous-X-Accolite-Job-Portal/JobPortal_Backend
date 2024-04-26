using Microsoft.AspNetCore.Identity;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class User : IdentityUser
    {
        public Guid? EmpId { get; set; }
        public Employee? Employee { get; set; }

        public Guid? CandidateId { get; set; }
        public Candidate? Candidate { get; set; }
    }
}
