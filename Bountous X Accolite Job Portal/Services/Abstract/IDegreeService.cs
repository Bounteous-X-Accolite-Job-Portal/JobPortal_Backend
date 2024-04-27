using Bountous_X_Accolite_Job_Portal.Models.DegreeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DegreeViewModel.DegreeResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IDegreeService
    {
        List<DegreeViewModel> GetAllDegree();
        DegreeResponseViewModel GetDegree(Guid id);
        Task<DegreeResponseViewModel> AddDegree(AddDegreeViewModel degree, Guid EmpId);
        Task<DegreeResponseViewModel> UpdateDegree(UpdateDegreeViewModel updateDegree);
        Task<DegreeResponseViewModel> RemoveDegree(Guid id);
    }
}
