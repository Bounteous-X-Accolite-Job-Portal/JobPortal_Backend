using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.EducationInstitutionViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IEducationInstitutionService
    {
        List<InstitutionViewModel> GetAllInstitutions();
        InstitutionResponseViewModel GetInstitution(Guid id);
        Task<InstitutionResponseViewModel> AddInstitution(AddInstitutionViewModel institution, Guid EmpId);
        Task<InstitutionResponseViewModel> UpdateInstution(UpdateInstitutionViewModel updateInstitution);
        Task<InstitutionResponseViewModel> RemoveInstitution(Guid id);
    }
}
