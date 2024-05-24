namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class SuccessfulApplicationsResponseViewModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public SuccessfulJobApplication successfulJobApplication { get; set; }  
    }
}
