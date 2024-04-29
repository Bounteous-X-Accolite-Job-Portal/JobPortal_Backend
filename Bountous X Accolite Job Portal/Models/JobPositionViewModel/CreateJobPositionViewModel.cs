namespace Bountous_X_Accolite_Job_Portal.Models.JobPositionViewModel
{
    public class CreateJobPositionViewModel
    {
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
    }
}
