namespace Bountous_X_Accolite_Job_Portal.Models.JobTypeViewModel.JobTypeResponseViewModel
{
    public class JobTypeViewModel
    {
        public string TypeName { get; set; }

        public JobTypeViewModel(JobType typ)
        {
            TypeName = typ.TypeName;
        }
    }
}
