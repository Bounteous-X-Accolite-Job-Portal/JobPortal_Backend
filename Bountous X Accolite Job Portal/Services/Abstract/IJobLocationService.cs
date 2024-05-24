using Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel.JobLocationResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJobLocationService
    {
        Task<AllJobLocationResponseViewModel> GetAllJobLocations();
        Task<JobLocationResponseViewModel> GetLocationById(Guid Id);
        Task<JobLocationResponseViewModel> AddLocation(CreateJobLocationViewModel location , Guid EmpId );
        Task<JobLocationResponseViewModel> UpdateLocation(EditJobLocationViewModel location);
        Task<JobLocationResponseViewModel> DeleteLocation(Guid locationId);
        
    }
}
