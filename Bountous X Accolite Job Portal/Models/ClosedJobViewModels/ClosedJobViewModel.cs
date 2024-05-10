namespace Bountous_X_Accolite_Job_Portal.Models.ClosedJobViewModels
{
    public class ClosedJobViewModel
    {
        public Guid ClosedJobId { get; set; }
        public string JobCode { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public DateTime LastDate { get; set; }
        public Guid? DegreeId { get; set; }
        public string Experience { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? PositionId { get; set; }
        public Guid? JobTypeId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? EmployeeId { get; set; }

        public ClosedJobViewModel(ClosedJob closedJob)
        {
            ClosedJobId = closedJob.ClosedJobId;
            JobCode = closedJob.JobCode;
            JobTitle = closedJob.JobTitle;
            JobDescription = closedJob.JobDescription;
            LastDate = closedJob.LastDate;
            DegreeId = closedJob.DegreeId;
            Experience = closedJob.Experience;
            CategoryId = closedJob.CategoryId;
            PositionId = closedJob.PositionId;
            JobTypeId = closedJob.JobTypeId;
            LocationId = closedJob.LocationId;
            EmployeeId = closedJob.EmployeeId;
        }
    }
}
