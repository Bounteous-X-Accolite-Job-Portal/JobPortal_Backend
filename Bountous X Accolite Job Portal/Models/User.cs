using Microsoft.AspNetCore.Identity;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class User : IdentityUser
    {
        public DateTime? ResetPasswordExpiry {  get; set; }
        public string? EmailToken {  get; set; }    
        public DateTime? EmailConfirmExpiry { get; set; }   

        public Guid? EmpId { get; set; }
        public virtual Employee? Employee { get; set; }

        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }

        public string? ResetPasswordToken { get; set; }

        public string? ChangePasswordToken {  get; set; }
        public DateTime? ChangePasswordExpiry {  get; set; }
        public string? ReferalToken { get; internal set; }

        public string? AutoPassword {  get; set; }
    }
}
