namespace Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel.ResponseViewModels
{
    public class CandidateEducationViewModel
    {
        public Guid EducationId { get; set; }
        public string? InstitutionOrSchoolName { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public double Grade { get; set; }
        public Guid? InstitutionId { get; set; }
        public Guid? DegreeId { get; set; }
        public Guid? CandidateId { get; set; }

        public CandidateEducationViewModel(CandidateEducation education)
        {
            EducationId = education.EducationId;
            InstitutionOrSchoolName = education.InstitutionOrSchoolName;
            StartYear = education.StartYear;
            EndYear = education.EndYear;
            Grade = education.Grade;
            InstitutionId = education.InstitutionId;
            DegreeId = education.DegreeId;
            CandidateId = education.CandidateId;
        }
    }
}
