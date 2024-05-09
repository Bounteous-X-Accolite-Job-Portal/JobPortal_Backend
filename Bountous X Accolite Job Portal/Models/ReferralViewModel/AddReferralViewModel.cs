namespace Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel
{
    public class AddReferralViewModel
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid JobId { get; set; }
    }
}
