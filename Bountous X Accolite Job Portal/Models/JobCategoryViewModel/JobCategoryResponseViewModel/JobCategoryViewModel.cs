namespace Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel.JobCategoryResponseViewModel
{
    public class JobCategoryViewModel
    {
        public Guid CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public JobCategoryViewModel(JobCategory jobCategory)
        {
            CategoryId = jobCategory.CategoryId;
            CategoryCode = jobCategory.CategoryCode;
            CategoryName = jobCategory.CategoryName;
            Description = jobCategory.Description;
        }
    }
}
