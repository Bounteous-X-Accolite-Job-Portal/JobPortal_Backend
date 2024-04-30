namespace Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel
{
    public class AddCandidateExperienceViewModel
    {
        public string ExperienceTitle { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool IsCurrentlyWorking { get; set; }
        public string Description { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
