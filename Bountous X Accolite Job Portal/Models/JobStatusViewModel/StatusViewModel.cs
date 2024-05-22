namespace Bountous_X_Accolite_Job_Portal.Models.JobStatusViewModel
{
    public class StatusViewModel
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public StatusViewModel(Status status)
        {
            StatusId = status.StatusId;
            StatusName = status.StatusName;
        }
    }
}