using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationModels;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationModels.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJobApplicationService
    {
        Task<JobApplicationResponseViewModel> GetJobApplicaionById(Guid Id);
        Task<JobApplicationResponseViewModel> GetClosedJobApplicaionById(Guid Id);
        Task<AllJobApplicationResponseViewModel> GetJobApplicationByCandidateId(Guid CandidateId);
        Task<AllJobApplicationResponseViewModel> GetJobApplicationByJobId(Guid JobId);
        Task<AllJobApplicationResponseViewModel> GetJobApplicationByClosedJobId(Guid ClosedJobId);
        Task<AllJobApplicationResponseViewModel> GetClosedJobApplicationByCandidateId(Guid CandidateId);
        Task<JobApplicationResponseViewModel> Apply(AddJobApplication jobApplication, Guid CandidateId);
        Task<JobApplicationResponseViewModel> ChangeJobApplicationStatus(Guid ApplicationId, int StatusId);
        Task<AllJobResponseViewModel> GetJobsAppliedByCandidateId(Guid CandidateId);
        Task<Boolean> IsCandidateApplicable(Guid JobId, Guid CandidateId);
        Task<AllApplicantResponseViewModel> GetApplicantsByJobId(Guid JobId);
        Task<AllApplicantResponseViewModel> GetApplicantsByClosedJobId(Guid JobId);
        Task<ResponseViewModel> SendOfferLetter(Guid Id);
        Task<SuccessfulApplicationsResponseViewModel> GetAllApplicationsWithSuccess();
    }
}