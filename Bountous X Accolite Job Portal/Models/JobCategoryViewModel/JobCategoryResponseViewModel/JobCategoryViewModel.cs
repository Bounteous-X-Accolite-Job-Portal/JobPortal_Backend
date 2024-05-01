namespace Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel.JobCategoryResponseViewModel
{
    public class JobCategoryViewModel
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public JobCategoryViewModel(JobCategory jobCategory)
        {
            CategoryCode = jobCategory.CategoryCode;
            CategoryName = jobCategory.CategoryName;
            Description = jobCategory.Description;
        }
    }
}
