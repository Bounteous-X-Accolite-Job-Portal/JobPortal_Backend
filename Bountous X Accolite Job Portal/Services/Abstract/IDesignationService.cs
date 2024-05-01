using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel.ResponseViewModels;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IDesignationService
    {
        Task<DesignationResponseViewModel> AddDesignation(AddDesignationViewModel designation, Guid empId);
    }
}
