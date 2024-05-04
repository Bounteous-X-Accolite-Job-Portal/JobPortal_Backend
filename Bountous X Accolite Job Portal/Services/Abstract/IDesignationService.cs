using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel.ResponseViewModels;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IDesignationService
    {
        bool HasPrivilege(string role);
        bool HasSpecialPrivilege(string role);
        Task<DesignationResponseViewModel> GetDesignationById(int Id);
        Task<DesignationResponseViewModel> AddDesignation(AddDesignationViewModel designation, Guid empId);
    }
}
