using Bountous_X_Accolite_Job_Portal.Models.DegreeModels.DegreeResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Models.EducationInstitutionViewModel;

namespace Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel.ResponseViewModels
{
    public class CompleteEducationViewModel
    {
        public CandidateEducationViewModel Education { get; set; }
        public InstitutionViewModel Institution { get; set; }
        public DegreeViewModel Degree { get; set; }
    }
}
