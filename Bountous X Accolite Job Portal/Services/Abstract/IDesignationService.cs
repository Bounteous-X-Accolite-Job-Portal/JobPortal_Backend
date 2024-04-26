using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IDesignationService
    {
        Task<bool> AddDesignation(AddDesignationViewModel designation, Guid empId);
    }
}
