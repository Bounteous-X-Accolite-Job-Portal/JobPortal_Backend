
namespace Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel.JobApplicationResponse
{
    public class JobApplicationResponseViewModel : ResponseViewModel
    {
        internal List<JobApplicationViewModel> Applications;

        public JobApplicationViewModel Application { get; set; }
    }
}
