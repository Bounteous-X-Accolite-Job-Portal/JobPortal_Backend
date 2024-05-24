﻿using Bountous_X_Accolite_Job_Portal.Models.InterviewViewModel.InterviewResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel.JobApplicationResponse;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Models;

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
        Boolean IsCandidateApplicable(Guid JobId, Guid CandidateId);
        Task<AllApplicantResponseViewModel> GetApplicantsByJobId(Guid JobId);
        Task<AllApplicantResponseViewModel> GetApplicantsByClosedJobId(Guid JobId);

        List<SuccessfulJobApplication> GetAllApplicationsWithSuccess();
    }
}