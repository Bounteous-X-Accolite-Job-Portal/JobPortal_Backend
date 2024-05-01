namespace Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel
{
    public class JobViewModel
    {
        public string JobCode { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public DateTime LastDate { get; set; }
        public Guid? DegreeId { get; set; }
        public string Experience { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? PositionId { get; set; }
        public Guid? JobType { get; set; }
        public Guid? LocationId { get; set; }
        
        public JobViewModel(Job job)
        {
            JobCode = job.JobCode;
            JobTitle = job.JobTitle;
            JobDescription = job.JobDescription;
            LastDate = job.LastDate;
            JobType = job.JobTypeId;
            LocationId = job.LocationId;
            DegreeId = job.DegreeId;
            Experience = job.Experience;
            CategoryId = job.CategoryId;
            PositionId = job.PositionId;
        }
    }
}
