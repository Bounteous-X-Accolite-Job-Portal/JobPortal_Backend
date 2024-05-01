using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel.JobCategoryResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobCategoryService : IJobCategoryService
    {
        private readonly ApplicationDbContext _context;

        public JobCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public AllJobCategoryResponseViewModel GetAllJobCategory()
        {
            List<JobCategory> list = _context.JobCategory.ToList();

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

        public JobCategoryResponseViewModel GetJobCategoryById(Guid categoryId)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();

            var category = _context.JobCategory.Find(categoryId);
            if (category == null)
            {
                response.Status = 404;
                response.Message = "Unable to Find Category !";
                return response;
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
            
            response.Status = 200;
            response.Message = "Successfully Added New Category !!";
            response.JobCategory = new JobCategoryViewModel(newCategory);
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
                dbcategory.Description = category.Description;

                _context.JobCategory.Update(dbcategory);
                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = "Category Successfully Updated";
                response.JobCategory = new JobCategoryViewModel(dbcategory);
            }
            else
            {
                response.Status = 404;
                response.Message = "Unable to Find Category !";
            }
            return response;
        }

        public async Task<JobCategoryResponseViewModel> DeleteJobCategory(Guid categoryId)
        {
            JobCategoryResponseViewModel response = new JobCategoryResponseViewModel();

            var category = _context.JobCategory.Find(categoryId);
            if (category != null)
            {
                _context.JobCategory.Remove(category);
                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = "Category Successfully Deleted !";
            }
            else
            {
                response.Status = 404;
                response.Message = "Unable to Delete Category !";
            }
            return response;
        }
    }
}
