namespace Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels
{
    public class UpdateCandidateViewModel
    {
        public Guid CandidateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }
        public string? AddressLine1 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
    }
}
