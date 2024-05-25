using Bountous_X_Accolite_Job_Portal.Models.EducationInstitutionViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IEducationInstitutionService
    {
        Task<List<InstitutionViewModel>> GetAllInstitutions();
        Task<InstitutionResponseViewModel> GetInstitution(Guid id);
        Task<InstitutionResponseViewModel> AddInstitution(AddInstitutionViewModel institution, Guid EmpId);
        Task<InstitutionResponseViewModel> UpdateInstution(UpdateInstitutionViewModel updateInstitution);
        Task<InstitutionResponseViewModel> RemoveInstitution(Guid id);
    }
}
