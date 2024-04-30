namespace Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModel
{
    public class CandidateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? AddressLine1 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }

        public CandidateViewModel(Candidate candidate)
        {
            FirstName = candidate.FirstName;
            LastName = candidate.LastName;
            Email = candidate.Email;
            Phone = candidate.Phone;
            AddressLine1 = candidate.AddressLine1;
            City = candidate.City;
            State = candidate.State;
            Country = candidate.Country;
            ZipCode = candidate.ZipCode;
        }
    }
}
