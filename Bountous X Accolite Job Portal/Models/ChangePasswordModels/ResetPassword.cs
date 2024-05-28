namespace Bountous_X_Accolite_Job_Portal.Models.ChangePasswordModels
{
    public class ResetPassword
    {
        public string? Email { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
