namespace Bountous_X_Accolite_Job_Portal.Models.JobPositionViewModel.JobPositionResponseViewModel
{
    public class DisplayJobPositionViewModel
    {
        public DisplayJobPositionViewModel()
        {
        }
        public DisplayJobPositionViewModel(JobPosition position)
        {
            PositionCode = position.PositionCode;
            PositionName = position.PositionName;
            Description = position.Description;
            CategoryId = position.CategoryId;
        }
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
    }
}
