using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel.JobLocationResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobLocationService : IJobLocationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;
        public JobLocationService(ApplicationDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<JobLocationResponseViewModel> AddLocation(CreateJobLocationViewModel location , Guid EmpId)
        {
            JobLocation newLocation = new JobLocation();
            newLocation.City = location.City;
            newLocation.Country = location.Country;
            newLocation.State = location.State;
            newLocation.EmpId = EmpId;
            newLocation.Address = location.Address;

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
                await _cache.RemoveAsync($"allJobLocations");

                response.Status = 200;
                response.Message = "Successfully Added New Location !!";
                response.jobLocation = new JobLocationViewModel(newLocation);
                return response;
            }

        }

        public async Task<JobLocationResponseViewModel> DeleteLocation(Guid locationId)
        {
            JobLocationResponseViewModel response = new JobLocationResponseViewModel();

            string key = $"getJobLocationById-{locationId}";
            string? getJobLocationByIdFromCache = await _cache.GetStringAsync(key);

            JobLocation loc;
            if (string.IsNullOrEmpty(getJobLocationByIdFromCache))
            {
                loc = _dbContext.JobLocation.Find(locationId);
                if (loc == null)
                {
                    response.Status = 404;
                    response.Message = "Location NOT Found !!";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(loc));
            }
            else
            {
                loc = JsonSerializer.Deserialize<JobLocation>(getJobLocationByIdFromCache);
            }

            _dbContext.JobLocation.Remove(loc);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"allJobLocations");
            await _cache.RemoveAsync($"getJobLocationById-{locationId}");

            response.Status = 200;
            response.Message = "Location Successfully Deleted !";
            response.jobLocation = new JobLocationViewModel(loc);
            return response;
        }

        public async Task<AllJobLocationResponseViewModel> GetAllJobLocations()
        {
            string key = $"allJobLocations";
            string? allJobLocationsFromCache = await _cache.GetStringAsync(key);

            List<JobLocation> list;
            if (string.IsNullOrEmpty(allJobLocationsFromCache))
            {
                list = _dbContext.JobLocation.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<JobLocation>>(allJobLocationsFromCache);
            }

            List<JobLocationViewModel> Locationlist = new List<JobLocationViewModel>();
            foreach (var location in list)
                Locationlist.Add(new JobLocationViewModel(location));

            AllJobLocationResponseViewModel response = new AllJobLocationResponseViewModel();

            response.Status = 200;
            response.Message = "Successfully reterived All Job Locations";
            response.allJobLocations = Locationlist;
            return response;
        }

        public async Task<JobLocationResponseViewModel> GetLocationById(Guid Id)
        {
            JobLocationResponseViewModel response = new JobLocationResponseViewModel();

            string key = $"getJobLocationById-{Id}";
            string? getJobLocationByIdFromCache = await _cache.GetStringAsync(key);

            JobLocation loc;
            if (string.IsNullOrEmpty(getJobLocationByIdFromCache))
            {
                loc = _dbContext.JobLocation.Find(Id);
                if(loc == null)
                {
                    response.Status = 500;
                    response.Message = "Location NOT reterived !!";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(loc));
            }
            else
            {
                loc = JsonSerializer.Deserialize<JobLocation>(getJobLocationByIdFromCache);
            }

            response.Status = 200;
            response.Message = "Location Successfully reterived !!";
            response.jobLocation = new JobLocationViewModel(loc);
            return response;
        }

        public async Task<JobLocationResponseViewModel> UpdateLocation(EditJobLocationViewModel location)
        {
            JobLocationResponseViewModel response = new JobLocationResponseViewModel();

            string key = $"getJobLocationById-{location.LocationId}";
            string? getJobLocationByIdFromCache = await _cache.GetStringAsync(key);

            JobLocation loc;
            if (string.IsNullOrEmpty(getJobLocationByIdFromCache))
            {
                loc = _dbContext.JobLocation.Find(location.LocationId);
                if (loc == null)
                {
                    response.Status = 404;
                    response.Message = "Location NOT Found !!";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(loc));
            }
            else
            {
                loc = JsonSerializer.Deserialize<JobLocation>(getJobLocationByIdFromCache);
            }

            loc.State = location.State;
            loc.City = location.City;
            loc.Country = location.Country;
            loc.Address = location.Address;

            _dbContext.JobLocation.Update(loc);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"allJobLocations");
            await _cache.RemoveAsync($"getJobLocationById-{loc.LocationId}");

            response.Status = 200;
            response.Message = "Location Details Updated !!";
            response.jobLocation = new JobLocationViewModel(loc);
            return response;
        }
    }
}
