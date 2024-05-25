using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobPositionViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobPositionViewModel.JobPositionResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobPositionService : IJobPositionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;
        public JobPositionService(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<JobPositionResponseViewModel> AddJobPosition(CreateJobPositionViewModel jobPosition , Guid EmpId)
        {
            JobPosition newPosition = new JobPosition();
            newPosition.PositionCode = jobPosition.PositionCode;
            newPosition.PositionName = jobPosition.PositionName;
            newPosition.Description = jobPosition.Description;
            newPosition.CategoryId = jobPosition.CategoryId;
            newPosition.EmpId = EmpId;
            
            await _context.JobPosition.AddAsync(newPosition);
            await _context.SaveChangesAsync();

            JobPositionResponseViewModel response = new JobPositionResponseViewModel();
            if (newPosition == null)
            {
                response.Status = 500;
                response.Message = "Unable to Add New Position";
            }
            else
            {
                await _cache.RemoveAsync($"allJobPositions");

                response.Status = 200;
                response.Message = "Successfully Added New Category !!";
                response.jobPosition = new JobPositionViewModel(newPosition);
            }
            
            return response;
        }

        public async Task<JobPositionResponseViewModel> DeleteJobPosition(Guid PositionId)
        {
            JobPositionResponseViewModel response = new JobPositionResponseViewModel();

            string key = $"getJobPositionById-{PositionId}";
            string? getJobPositionByIdFromCache = await _cache.GetStringAsync(key);

            JobPosition position;
            if (string.IsNullOrWhiteSpace(getJobPositionByIdFromCache))
            {
                position = _context.JobPosition.Find(PositionId);
                if (position == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Find Position";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(position));
            }
            else
            {
                position = JsonSerializer.Deserialize<JobPosition>(getJobPositionByIdFromCache);
            }

            _context.JobPosition.Remove(position);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"allJobPositions");
            await _cache.RemoveAsync($"getJobPositionById-{PositionId}");

            response.Status = 200;
            response.Message = "Position Deleted Successfully !";
            response.jobPosition = new JobPositionViewModel(position);
            return response;
        }

        public async Task<AllJobPositionResponseViewModel> GetAllJobPositions()
        {
            string key = $"allJobPositions";
            string? allJobPositionsFromCache = await _cache.GetStringAsync(key);

            List<JobPosition> list;
            if (string.IsNullOrWhiteSpace(allJobPositionsFromCache))
            {
                list = _context.JobPosition.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<JobPosition>>(allJobPositionsFromCache);
            }

            List<JobPositionViewModel> jobPositions = new List<JobPositionViewModel>();
            foreach (JobPosition jobPosition in list)
                jobPositions.Add(new JobPositionViewModel(jobPosition));

            AllJobPositionResponseViewModel response = new AllJobPositionResponseViewModel();
            response.allJobPositions = jobPositions;
            response.Status = 200;
            response.Message = "Successfully reterived Positions";
            return response;
        }

        public async Task<JobPositionResponseViewModel> GetJobPositionById(Guid PositionId)
        {
            JobPositionResponseViewModel response = new JobPositionResponseViewModel();

            string key = $"getJobPositionById-{PositionId}";
            string? getJobPositionByIdFromCache = await _cache.GetStringAsync(key);

            JobPosition position;
            if (string.IsNullOrWhiteSpace(getJobPositionByIdFromCache))
            {
                position = _context.JobPosition.Find(PositionId);
                if(position == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Find Position";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(position));
            }
            else
            {
                position = JsonSerializer.Deserialize<JobPosition>(getJobPositionByIdFromCache);
            }

            response.Status = 200;
            response.Message = "Position Successfully Reterived";
            response.jobPosition = new JobPositionViewModel(position);
            return response;
        }

        public async Task<JobPositionResponseViewModel> UpdateJobPosition(EditJobPositionViewModel jobPosition)
        {
            JobPositionResponseViewModel response = new JobPositionResponseViewModel();

            string key = $"getJobPositionById-{jobPosition.PositionId}";
            string? getJobPositionByIdFromCache = await _cache.GetStringAsync(key);

            JobPosition dbposition;
            if (string.IsNullOrWhiteSpace(getJobPositionByIdFromCache))
            {
                dbposition = _context.JobPosition.Find(jobPosition.PositionId);
                if (dbposition == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Find Position";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(dbposition));
            }
            else
            {
                dbposition = JsonSerializer.Deserialize<JobPosition>(getJobPositionByIdFromCache);
            }

            dbposition.PositionCode = jobPosition.PositionCode;
            dbposition.PositionName = jobPosition.PositionName;
            dbposition.Description = jobPosition.Description;
            dbposition.CategoryId = jobPosition.CategoryId;

            _context.JobPosition.Update(dbposition);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"allJobPositions");
            await _cache.RemoveAsync($"getJobPositionById-{jobPosition.PositionId}");

            response.Status = 200;
            response.Message = "Position Successfully Updated";
            response.jobPosition = new JobPositionViewModel(dbposition);
            return response;
        }
    }
}
