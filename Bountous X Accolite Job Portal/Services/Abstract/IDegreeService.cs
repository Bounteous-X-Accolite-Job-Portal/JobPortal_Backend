using Bountous_X_Accolite_Job_Portal.Models.DegreeModels;
using Bountous_X_Accolite_Job_Portal.Models.DegreeModels.DegreeResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IDegreeService
    {
        Task<List<DegreeViewModel>> GetAllDegree();
        Task<DegreeResponseViewModel> GetDegree(Guid id);
        Task<DegreeResponseViewModel> AddDegree(AddDegreeViewModel degree, Guid EmpId);
        Task<DegreeResponseViewModel> UpdateDegree(UpdateDegreeViewModel updateDegree);
        Task<DegreeResponseViewModel> RemoveDegree(Guid id);
    }
}
