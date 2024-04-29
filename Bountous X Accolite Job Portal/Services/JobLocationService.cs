using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel.JobLocationResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobLocationService : IJobLocation
    {
        private readonly ApplicationDbContext _dbContext;

        public JobLocationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JobLocationResponseViewModel> AddLocation(CreateJobLocationViewModel location , Guid EmpId)
        {
            JobLocation newLocation = new JobLocation();
            newLocation.City = location.City;
            newLocation.Country = location.Country;
            newLocation.State = location.State;
            newLocation.EmpId = EmpId;

            await _dbContext.JobLocation.AddAsync(newLocation);
            await _dbContext.SaveChangesAsync();

            JobLocationResponseViewModel response = new JobLocationResponseViewModel();
            if (newLocation == null)
            {
                response.Status = 500;
                response.Message = "Unable to Add New Location";
                return response;
            }
            else
            {
                response.Status = 200;
                response.Message = "Successfully Added New Location !!";
                response.jobLocation = new DisplayJobLocationViewModel(newLocation);
                return response;
            }

        }

        public async Task<JobLocationResponseViewModel> DeleteLocation(Guid locationId)
        {
            JobLocationResponseViewModel response = new JobLocationResponseViewModel();

            var loc = _dbContext.JobLocation.Find(locationId);
            if (loc != null)
            {
                _dbContext.JobLocation.Remove(loc);
                await _dbContext.SaveChangesAsync();
                response.Status = 200;
                response.Message = "Location Successfully Deleted !";
            }
            else
            {
                response.Status = 500;
                response.Message = "Unable to Delete Location";
            }
            return response;
        }

        public AllJobLocationResponseViewModel GetAllJobLocations()
        {
            List<JobLocation> list = _dbContext.JobLocation.ToList();
            List<DisplayJobLocationViewModel> Locationlist = new List<DisplayJobLocationViewModel>();
            foreach (var location in list)
                Locationlist.Add(new DisplayJobLocationViewModel(location));

            AllJobLocationResponseViewModel response = new AllJobLocationResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully reterived All Job Locations";
            response.allJobLocations = Locationlist;
            return response;
        }

        public JobLocationResponseViewModel GetLocationById(Guid Id)
        {
            JobLocationResponseViewModel response = new JobLocationResponseViewModel();
            var loc = _dbContext.JobLocation.Find(Id);
            if (loc != null)
            {
                response.Status = 200;
                response.Message = "Location Successfully reterived !!";
                response.jobLocation = new DisplayJobLocationViewModel(loc);
            }
            else
            {
                response.Status = 500;
                response.Message = "Location NOT reterived !!";
            }
            return response;
        }

        public async Task<JobLocationResponseViewModel> UpdateLocation(EditJobLocationViewModel location)
        {
            JobLocationResponseViewModel response = new JobLocationResponseViewModel();
            var loc = _dbContext.JobLocation.Find(location.LocationId);
            if (loc != null)
            {
                loc.State = location.State;
                loc.City = location.City;
                loc.Country = location.Country;

                _dbContext.JobLocation.Update(loc);
                await _dbContext.SaveChangesAsync();

                response.Status = 200;
                response.Message = "Location Details Updated !!";
            }
            else
            {
                response.Status = 404;
                response.Message = "Location NOT Found !!";
            }

            return response;
        }
    }
}
