namespace Bountous_X_Accolite_Job_Portal.Models.JobCategoryViewModel.JobCategoryResponseViewModel
{
    public class DisplayJobCategoryViewModel
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }

        public DisplayJobCategoryViewModel()
        {
        }
        public DisplayJobCategoryViewModel(JobCategory jobCategory)
        {
            CategoryCode = jobCategory.CategoryCode;
            CategoryName = jobCategory.CategoryName;
        }


    }
}
