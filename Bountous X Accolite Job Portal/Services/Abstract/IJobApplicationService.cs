using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel.JobApplicationResponse;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJobApplicationService
    {
        JobApplicationResponseViewModel GetJobApplicaionById(Guid Id);
        AllJobApplicationResponseViewModel GetJobApplicationByCandidateId(Guid CandidateId);
        AllJobApplicationResponseViewModel GetJobApplicationByJobId(Guid JobId);
        AllJobApplicationResponseViewModel GetJobApplicationByClosedJobId(Guid ClosedJobId);
        AllJobApplicationResponseViewModel GetClosedJobApplicationByCandidateId(Guid CandidateId);
        Task<JobApplicationResponseViewModel> Apply(AddJobApplication jobApplication, Guid CandidateId);
        Task<JobApplicationResponseViewModel> ChangeJobApplicationStatus(Guid ApplicationId, int StatusId);
        AllJobResponseViewModel GetJobsAppliedByCandidateId(Guid CandidateId);
        Boolean IsCandidateApplicable(Guid JobId , Guid CandidateId);
    }
}
