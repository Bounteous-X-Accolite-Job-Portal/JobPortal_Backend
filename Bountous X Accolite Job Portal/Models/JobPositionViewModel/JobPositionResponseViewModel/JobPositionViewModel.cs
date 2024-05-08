namespace Bountous_X_Accolite_Job_Portal.Models.JobPositionViewModel.JobPositionResponseViewModel
{
    public class JobPositionViewModel
    {
        public Guid PositionId { get; set; }
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }

        public JobPositionViewModel(JobPosition position)
        {
            PositionId = position.PositionId;
            PositionCode = position.PositionCode;
            PositionName = position.PositionName;
            Description = position.Description;
            CategoryId = position.CategoryId;
        }
    }
}
