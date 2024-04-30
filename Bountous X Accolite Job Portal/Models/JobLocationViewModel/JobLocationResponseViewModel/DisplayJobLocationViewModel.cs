namespace Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel.JobLocationResponseViewModel
{
    public class DisplayJobLocationViewModel
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public Guid? EmpId { get; set; }

        public DisplayJobLocationViewModel()
        {
        }
        public DisplayJobLocationViewModel(JobLocation loc)
        {
            City = loc.City;
            State = loc.State;
            Country = loc.Country;
            EmpId = loc.EmpId;
        }
    }
}
