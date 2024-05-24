using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobTypeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobTypeViewModel.JobTypeResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobTypeService : IJobTypeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;
        public JobTypeService(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<JobTypeResponseViewModel> AddJobType(CreateJobTypeViewModel jobType, Guid EmpId)
        {
            JobType newJobType = new JobType(jobType);
            newJobType.EmpId = EmpId;

            await _context.JobType.AddAsync(newJobType);
            await _context.SaveChangesAsync();

            JobTypeResponseViewModel response = new JobTypeResponseViewModel();
            if(newJobType==null)
            {
                response.Status = 500;
                response.Message = "Unable to Add New Job Type !";
            }
            else
            {
                await _cache.RemoveAsync($"allJobTypes");

                response.Status = 200;
                response.Message = "Successfully Added New Job Type !";
                response.jobType = new JobTypeViewModel(newJobType);
            }
            return response;
        }

        public async Task<JobTypeResponseViewModel> DeleteJobType(Guid Id)
        {
            JobTypeResponseViewModel response = new JobTypeResponseViewModel();

            string key = $"getJobTypeById-{Id}";
            string? getJobTypesByIdFromCache = await _cache.GetStringAsync(key);

            JobType jobType;
            if (string.IsNullOrEmpty(getJobTypesByIdFromCache))
            {
                jobType = _context.JobType.Find(Id);
                if (jobType == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Delete Category !";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(jobType));
            }
            else
            {
                jobType = JsonSerializer.Deserialize<JobType>(getJobTypesByIdFromCache);
            }

            _context.JobType.Remove(jobType);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"allJobTypes");
            await _cache.RemoveAsync($"getJobTypeById-{Id}");

            response.Status = 200;
            response.Message = "Job Type Successfully Deleted !";
            return response;
        }

        public async Task<AllJobTypeResponseViewModel> GetAllJobTypes()
        {
            string key = $"allJobTypes";
            string? allJobTypesFromCache = await _cache.GetStringAsync(key);

            List<JobType> list;
            if (string.IsNullOrEmpty(allJobTypesFromCache))
            {
                list = _context.JobType.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<JobType>>(allJobTypesFromCache);
            }

            List<JobTypeViewModel> jobTypelist = new List<JobTypeViewModel>();
            
            foreach (var jobType in list)
            {
                jobTypelist.Add(new JobTypeViewModel(jobType));
            }

            AllJobTypeResponseViewModel response = new AllJobTypeResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully reterived Job Types";
            response.allJobTypes = jobTypelist;

            return response;
        }

        public async Task<JobTypeResponseViewModel> GetJobTypeById(Guid Id)
        {
            JobTypeResponseViewModel response = new JobTypeResponseViewModel();

            string key = $"getJobTypeById-{Id}";
            string? getJobTypesByIdFromCache = await _cache.GetStringAsync(key);

            JobType jobType;
            if (string.IsNullOrEmpty(getJobTypesByIdFromCache))
            {
                jobType = _context.JobType.Find(Id);
                if (jobType == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Find Job Type !";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(jobType));
            }
            else
            {
                jobType = JsonSerializer.Deserialize<JobType>(getJobTypesByIdFromCache);
            }

            response.Status = 200;
            response.Message = "Job Type Successfully Reterived !";
            response.jobType = new JobTypeViewModel(jobType);
            return response;
        }

        public async Task<JobTypeResponseViewModel> UpdateJobType(EditJobTypeViewModel jobType)
        {
            JobTypeResponseViewModel response = new JobTypeResponseViewModel();

            string key = $"getJobTypeById-{jobType.JobTypeId}";
            string? getJobTypesByIdFromCache = await _cache.GetStringAsync(key);

            JobType dbjobType;
            if (string.IsNullOrEmpty(getJobTypesByIdFromCache))
            {
                dbjobType = _context.JobType.Find(jobType.JobTypeId);
                if (dbjobType == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Update Job Type !";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(dbjobType));
            }
            else
            {
                dbjobType = JsonSerializer.Deserialize<JobType>(getJobTypesByIdFromCache);
            }

            dbjobType.TypeName = jobType.TypeName;

            _context.JobType.Update(dbjobType);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"allJobTypes");
            await _cache.RemoveAsync($"getJobTypeById-{dbjobType.JobTypeId}");

            response.Status = 200;
            response.Message = "Job Type Successfully Updated !";

            response.jobType = new JobTypeViewModel(dbjobType);
            return response;
        }
    }
}
