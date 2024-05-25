using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobStatusViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobStatusService : IJobStatusService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;
        public JobStatusService(ApplicationDbContext applicationDbContext, IDistributedCache cache)
        {
            _dbContext = applicationDbContext;
            _cache = cache;
        }

        public async Task<int> getInitialReferralStatus()
        {
            int statusId = -1;

            string key = $"allStatus";
            string? allStatusFromCache = await _cache.GetStringAsync(key);

            List<Status> status;
            if (string.IsNullOrWhiteSpace(allStatusFromCache))
            {
                status = _dbContext.Status.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(status));
            }
            else
            {
                status = JsonSerializer.Deserialize<List<Status>>(allStatusFromCache);
            }

            foreach(Status s in status)
            {
                if (String.Equals(s.StatusName.ToLower(), "referred"))
                {
                    statusId = s.StatusId;
                }
            }

            return statusId;
        }
        public async Task<int> getInitialSuccesstatus()
        {
            int statusId = -1;

            string key = $"allStatus";
            string? allStatusFromCache = await _cache.GetStringAsync(key);

            List<Status> status;
            if (string.IsNullOrWhiteSpace(allStatusFromCache))
            {
                status = _dbContext.Status.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(status));
            }
            else
            {
                status = JsonSerializer.Deserialize<List<Status>>(allStatusFromCache);
            }

            foreach (Status s in status)
            {
                if (String.Equals(s.StatusName.ToLower(), "success"))
                {
                    statusId = s.StatusId;
                }
            }

            return statusId;
        }
        public async Task<int> GetInitialApplicationStatus()
        {
            int statusId = -1;

            string key = $"allStatus";
            string? allStatusFromCache = await _cache.GetStringAsync(key);

            List<Status> status;
            if (string.IsNullOrWhiteSpace(allStatusFromCache))
            {
                status = _dbContext.Status.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(status));
            }
            else
            {
                status = JsonSerializer.Deserialize<List<Status>>(allStatusFromCache);
            }

            foreach (Status s in status)
            {
                if (String.Equals(s.StatusName.ToLower(), "applied"))
                {
                    statusId = s.StatusId;
                }
            }

            return statusId;
        }

        public async Task<bool> IsRejectedStatus(int StatusId)
        {
            string key = $"getStatusById-{StatusId}";
            string? getStatusFromCache = await _cache.GetStringAsync(key);

            Status status;
            if (string.IsNullOrWhiteSpace(getStatusFromCache))
            {
                status = _dbContext.Status.Find(StatusId);
                if(status == null)
                {
                    return false;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(status));
            }
            else
            {
                status = JsonSerializer.Deserialize<Status>(getStatusFromCache);
            }

            if (String.Equals(status.StatusName.ToLower(), "rejected"))
            {
                return true;
            }

            return false;
        }

        public async Task<ResponseViewModel> AddStatus(AddJobStatusViewModel jobstatus, Guid empId)
        {
            ResponseViewModel response = new ResponseViewModel();

            if (jobstatus == null || jobstatus.StatusName == null)
            {
                response.Status = 400;
                response.Message = "Invalid !!";
                return response;
            }

            Status addJobStatus = new Status();
            addJobStatus.StatusName = jobstatus.StatusName;
            addJobStatus.CreatedAt = DateTime.Now;
            addJobStatus.EmpId = empId;

            await _dbContext.Status.AddAsync(addJobStatus);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"allStatus");

            response.Status = 200;
            response.Message = "Successfully added status";
            return response;
        }

        public async Task<JobStatusResponseViewModel> GetStatusById(int statusId)
        {
            JobStatusResponseViewModel response = new JobStatusResponseViewModel();

            string key = $"getStatusById-{statusId}";
            string? getStatusFromCache = await _cache.GetStringAsync(key);

            Status status;
            if (string.IsNullOrWhiteSpace(getStatusFromCache))
            {
                status = _dbContext.Status.Find(statusId);
                if (status == null)
                {
                    response.Status = 404;
                    response.Message = "Status with this Id does not exist.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(status));
            }
            else
            {
                status = JsonSerializer.Deserialize<Status>(getStatusFromCache);
            }

            response.Status = 200;
            response.Message = "Successfully retrieved status with this Id.";
            response.StatusViewModel = new StatusViewModel(status);
            return response;
        }

        public async Task<AllStatusResponseViewModel> GetAllStatus()
        {
            AllStatusResponseViewModel response = new AllStatusResponseViewModel();

            string key = $"allStatus";
            string? allStatusFromCache = await _cache.GetStringAsync(key);

            List<Status> l;
            if (string.IsNullOrWhiteSpace(allStatusFromCache))
            {
                l = _dbContext.Status.Where(ite => true).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(l));
            }
            else
            {
                l = JsonSerializer.Deserialize<List<Status>>(allStatusFromCache);
            }

            List<StatusViewModel> list = new List<StatusViewModel>();
            foreach (var item in l)
            {
                list.Add(new StatusViewModel(item));
            }

            response.Status = 200;
            response.Message = "Successfully retrived all status.";
            response.AllStatus = list;
            return response;
        }

        public async Task<ResponseViewModel> DeleteStatus(int statusId)
        {
            ResponseViewModel response = new ResponseViewModel();

            string key = $"getStatusById-{statusId}";
            string? getStatusFromCache = await _cache.GetStringAsync(key);

            Status status;
            if (string.IsNullOrWhiteSpace(getStatusFromCache))
            {
                status = _dbContext.Status.Find(statusId);
                if (status == null)
                {
                    response.Status = 404;
                    response.Message = "Status with this Id does not exist.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(status));
            }
            else
            {
                status = JsonSerializer.Deserialize<Status>(getStatusFromCache);
            }
            
            _dbContext.Status.Remove(status);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"allStatus");
            await _cache.RemoveAsync($"getStatusById-{statusId}");

            response.Status = 200;
            response.Message = "Successfully removed this Status !!.";
            return response;
        }
    }
}