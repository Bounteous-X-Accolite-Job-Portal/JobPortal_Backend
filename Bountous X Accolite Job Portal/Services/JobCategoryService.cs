using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel.JobCategoryResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobCategoryService : IJobCategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;
        public JobCategoryService(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<AllJobCategoryResponseViewModel> GetAllJobCategory()
        {
            string key = $"allJobCategory";
            string? allJobCategoryFromCache = await _cache.GetStringAsync(key);

            List<JobCategory> list;
            if (string.IsNullOrWhiteSpace(allJobCategoryFromCache))
            {
                list = _context.JobCategory.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<JobCategory>>(allJobCategoryFromCache);
            }

            List<JobCategoryViewModel> categoryList = new List<JobCategoryViewModel>();
            foreach (JobCategory category in list)
            {
                categoryList.Add(new JobCategoryViewModel(category));
            }

            AllJobCategoryResponseViewModel response = new AllJobCategoryResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully reterived Categories";
            response.AllJobCategory = categoryList;
            return response;
        }

        public async Task<JobCategoryResponseViewModel> GetJobCategoryById(Guid categoryId)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();

            string key = $"getJobCategoryById-{categoryId}";
            string? getJobCategoryFromCache = await _cache.GetStringAsync(key);

            JobCategory category;
            if (string.IsNullOrWhiteSpace(getJobCategoryFromCache))
            {
                category = _context.JobCategory.Find(categoryId);
                if (category == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Find Category !";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(category));
            }
            else
            {
                category = JsonSerializer.Deserialize<JobCategory>(getJobCategoryFromCache);
            }

            response.Status = 200;
            response.Message = "Category Successfully Reterived";
            response.JobCategory = new JobCategoryViewModel(category);
            return response;
        }

        public async Task<JobCategoryResponseViewModel> AddJobCategory(CreateJobCategoryViewModel category, Guid EmpId)
        {
            JobCategory newCategory = new JobCategory();
            newCategory.EmpId = EmpId;
            newCategory.CategoryCode = category.CategoryCode;
            newCategory.CategoryName = category.CategoryName; 
            newCategory.Description = category.Description;

            await _context.JobCategory.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();
            if (newCategory == null)
            {
                response.Status = 500;
                response.Message = "Unable to Add New Category";
                return response;
            }

            await _cache.RemoveAsync($"allJobCategory");

            response.Status = 200;
            response.Message = "Successfully Added New Category !!";
            response.JobCategory = new JobCategoryViewModel(newCategory);
            return response;
        }

        public async Task<JobCategoryResponseViewModel> UpdateJobCategory(EditJobCategoryViewModel category)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();

            string key = $"getJobCategoryById-{category.CategoryId}";
            string? getJobCategoryFromCache = await _cache.GetStringAsync(key);

            JobCategory dbcategory;
            if (string.IsNullOrWhiteSpace(getJobCategoryFromCache))
            {
                dbcategory = _context.JobCategory.Find(category.CategoryId);
                if (dbcategory == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Find Category !";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(dbcategory));
            }
            else
            {
                dbcategory = JsonSerializer.Deserialize<JobCategory>(getJobCategoryFromCache);
            }

            dbcategory.CategoryName = category.CategoryName;
            dbcategory.CategoryCode = category.CategoryCode;
            dbcategory.Description = category.Description;

            _context.JobCategory.Update(dbcategory);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"allJobCategory");
            await _cache.RemoveAsync($"getJobCategoryById-{category.CategoryId}");

            response.Status = 200;
            response.Message = "Category Successfully Updated";
            response.JobCategory = new JobCategoryViewModel(dbcategory);
            return response;
        }

        public async Task<JobCategoryResponseViewModel> DeleteJobCategory(Guid categoryId)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();

            string key = $"getJobCategoryById-{categoryId}";
            string? getJobCategoryFromCache = await _cache.GetStringAsync(key);

            JobCategory category;
            if (string.IsNullOrWhiteSpace(getJobCategoryFromCache))
            {
                category = _context.JobCategory.Find(categoryId);
                if (category == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Find Category !";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(category));
            }
            else
            {
                category = JsonSerializer.Deserialize<JobCategory>(getJobCategoryFromCache);
            }

            _context.JobCategory.Remove(category);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"allJobCategory");
            await _cache.RemoveAsync($"getJobCategoryById-{categoryId}");

            response.Status = 200;
            response.Message = "Category Successfully Deleted !";
            return response;
        }
    }
}
