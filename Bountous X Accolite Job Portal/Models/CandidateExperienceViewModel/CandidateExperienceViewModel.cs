namespace Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel
{
    public class CandidateExperienceViewModel
    {
        public Guid ExperienceId { get; set; }
        public string ExperienceTitle { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool IsCurrentlyWorking { get; set; }
        public string Description { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? CandidateId { get; set; }

        public CandidateExperienceViewModel(CandidateExperience experience)
        {
            ExperienceId = experience.ExperienceId;
            ExperienceTitle = experience.ExperienceTitle;
            StartDate = experience.StartDate;
            EndDate = experience.EndDate;
            IsCurrentlyWorking = experience.IsCurrentlyWorking;
            Description = experience.Description;
            CompanyId = experience.CompanyId;
            CandidateId = experience.CandidateId;
        }
    }
}
