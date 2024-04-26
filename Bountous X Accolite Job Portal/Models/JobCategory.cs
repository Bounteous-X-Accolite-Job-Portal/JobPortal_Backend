namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class JobCategory
    {
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; }

        public Guid JobId { get; set; }
        public Job Job { get; set; }
    }
}
