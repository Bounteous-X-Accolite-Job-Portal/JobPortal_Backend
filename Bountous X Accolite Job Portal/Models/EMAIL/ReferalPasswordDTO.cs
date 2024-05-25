namespace Bountous_X_Accolite_Job_Portal.Models.EMAIL
{
    public record ReferalPasswordDTO
    {
        public string Email { get; set; }   
        public string ReferalToken { get; set; }

        public string AutoPassword { get; set; }
    }
}
