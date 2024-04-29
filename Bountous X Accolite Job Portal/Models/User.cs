using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class User : IdentityUser
    {
        public Guid? EmpId { get; set; }
        public virtual Employee? Employee { get; set; }

        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }

        
    }
}
