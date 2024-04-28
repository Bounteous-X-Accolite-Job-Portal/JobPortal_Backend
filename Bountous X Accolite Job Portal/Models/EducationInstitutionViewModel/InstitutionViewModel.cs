namespace Bountous_X_Accolite_Job_Portal.Models.EducationInstitutionViewModel
{
    public class InstitutionViewModel
    {
        public Guid InstitutionId { get; set; }
        public string InstitutionOrSchool { get; set; }
        public string UniversityOrBoard { get; set; }
        public Guid? EmpId { get; set; }

        public InstitutionViewModel(EducationInstitution institution)
        {
            InstitutionId = institution.InstitutionId;
            InstitutionOrSchool = institution.InstitutionOrSchool;
            UniversityOrBoard = institution.UniversityOrBoard;
            EmpId = institution.EmpId;
        }
    }
}
