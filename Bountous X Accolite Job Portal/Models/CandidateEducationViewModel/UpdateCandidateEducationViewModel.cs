namespace Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel
{
    public class UpdateCandidateEducationViewModel
    {
        public Guid EducationId { get; set; }
        public string? InstitutionOrSchoolName { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public double Grade { get; set; }
        public Guid? InstitutionId { get; set; }
        public Guid? DegreeId { get; set; }
    }
}
