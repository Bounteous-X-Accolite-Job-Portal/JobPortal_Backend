namespace Bountous_X_Accolite_Job_Portal.Models
{
    public record ResetPasswordDTO
    {
        public string Email { get; set; }   
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
