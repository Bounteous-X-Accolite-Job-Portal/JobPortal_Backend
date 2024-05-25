namespace Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel.JobLocationResponseViewModel
{
    public class JobLocationViewModel
    {
        public Guid LocationId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public Guid? EmpId { get; set; }

        public JobLocationViewModel(JobLocation loc)
        {
            LocationId = loc.LocationId;
            City = loc.City;
            State = loc.State;
            Country = loc.Country;
            EmpId = loc.EmpId;
            Address = loc.Address;
        }
    }
}
