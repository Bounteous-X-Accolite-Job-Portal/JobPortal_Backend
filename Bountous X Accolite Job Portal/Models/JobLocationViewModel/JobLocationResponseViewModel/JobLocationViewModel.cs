namespace Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel.JobLocationResponseViewModel
{
    public class JobLocationViewModel
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public Guid? EmpId { get; set; }

        public JobLocationViewModel(JobLocation loc)
        {
            City = loc.City;
            State = loc.State;
            Country = loc.Country;
            EmpId = loc.EmpId;
            Address = loc.Address;
        }
    }
}
