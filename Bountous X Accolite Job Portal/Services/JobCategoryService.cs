using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel.JobCategoryResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobLocationViewModel.JobLocationResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobCategoryService : IJobCategory
    {
        private readonly ApplicationDbContext _context;

        public JobCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobCategoryResponseViewModel> AddJobCategory(CreateJobCategoryViewModel category, Guid EmpId)
        {
            JobCategory newCategory = new JobCategory();
            newCategory.EmpId = EmpId;
            newCategory.CategoryCode = category.CategoryCode;
            newCategory.CategoryName = category.CategoryName;   

            await _context.JobCategory.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();
            if (newCategory == null)
            {
                response.Status = 500;
                response.Message = "Unable to Add New Category";
                return response;
            }
            else
            {
                response.Status = 200;
                response.Message = "Successfully Added New Category !!";
                response.jobCategory = new DisplayJobCategoryViewModel(newCategory);
                return response;
            }
        }

        public async Task<JobCategoryResponseViewModel> DeleteJobCategory(Guid categoryId)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();
            var category = _context.JobCategory.Find(categoryId);
            if(category != null)
            {
                _context.JobCategory.Remove(category);
                await _context.SaveChangesAsync();
                response.Status = 200;
                response.Message = "Category Successfully Deleted !";
            }
            else
            {
                response.Status = 500;
                response.Message = "Unable to Delete Category !";
            }
            return response;
        }

        public AllJobCategoryResponseViewModel GetAllJobCategory()
        {
            List<JobCategory> list = _context.JobCategory.ToList();
            List<DisplayJobCategoryViewModel> categoryList = new List<DisplayJobCategoryViewModel>();
            foreach (JobCategory category in list)
                categoryList.Add(new DisplayJobCategoryViewModel(category));

            AllJobCategoryResponseViewModel response = new AllJobCategoryResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully reterived Categories";
            response.allJobCategory = categoryList;
            return response;
        }

        public JobCategoryResponseViewModel GetJobCategoryById(Guid categoryId)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();
            var category = _context.JobCategory.Find(categoryId);
            if(category != null)
            {
                response.Status = 200;
                response.Message = "Category Successfully Reterived";
                response.jobCategory = new DisplayJobCategoryViewModel(category);
            }
            else
            {
                response.Status = 500;
                response.Message = "Unable to Find Category !";
            }
            return response;
        }

        public async Task<JobCategoryResponseViewModel> UpdateJobCategory(EditJobCategoryViewModel category)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();
            var dbcategory = _context.JobCategory.Find(category.CategoryId);
            if (dbcategory != null)
            {
                dbcategory.CategoryName = category.CategoryName;
                dbcategory.CategoryCode = category.CategoryCode;

                _context.JobCategory.Update(dbcategory);
                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = "Category Successfully Updated";
                response.jobCategory = new DisplayJobCategoryViewModel(dbcategory);
            }
            else
            {
                response.Status = 404;
                response.Message = "Unable to Find Category !";
            }
            return response;
        }
    }
}
