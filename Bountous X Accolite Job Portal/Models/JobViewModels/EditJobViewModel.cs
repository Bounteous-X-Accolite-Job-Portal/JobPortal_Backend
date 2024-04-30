namespace Bountous_X_Accolite_Job_Portal.Models.JobViewModels
{
    public class EditJobViewModel
    {
        public Guid JobId { get; set; }
        public string JobCode { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public DateTime LastDate { get; set; }
        public string Experience { get; set; }
        public Guid? DegreeId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? PositionId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? JobType { get; set; }
    }
}
