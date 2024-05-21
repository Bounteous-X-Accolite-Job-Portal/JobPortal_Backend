namespace Bountous_X_Accolite_Job_Portal.Models.JobTypeViewModel.JobTypeResponseViewModel
{
    public class JobTypeViewModel
    {
        public Guid JobTypeId { get; set; }
        public string TypeName { get; set; }

        public JobTypeViewModel(JobType typ)
        {
            JobTypeId = typ.JobTypeId;
            TypeName = typ.TypeName;
        }
    }
}
