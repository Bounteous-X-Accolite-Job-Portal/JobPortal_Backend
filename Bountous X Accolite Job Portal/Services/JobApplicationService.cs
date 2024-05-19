using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IJobStatusService _jobStatusService;
        private readonly I_InterviewService _interviewService;
        private readonly IJobService _jobService;
        private readonly ICandidateAccountService _candidateAccountService;
        private readonly ISkillsService _skillsService;
        private readonly IResumeService _resumeService;
        private readonly IJobStatusService _statusService;
        private readonly ICandidateEducationService _candidateEducationService;
        private readonly ICandidateExperienceService _candidateExperienceService;
        private readonly IEducationInstitutionService _educationInstitutionService;
        private readonly IDegreeService _degreeService;
        private readonly ICompanyService _companyService;
        public JobApplicationService(
            ApplicationDbContext applicationDbContext, 
            IJobStatusService jobStatusService, 
            I_InterviewService interviewService, 
            IJobService jobService, 
            ICandidateAccountService candidateAccountService, 
            ISkillsService skillsService, 
            IResumeService resumeService,
            IJobStatusService statusService,
            ICandidateExperienceService candidateExperienceService,
            ICandidateEducationService candidateEducationService,
            IEducationInstitutionService educationInstitutionService,
            IDegreeService degreeService,
            ICompanyService companyService
        )
        {
            _dbContext = applicationDbContext;
            _jobStatusService = jobStatusService;
            _interviewService = interviewService;
            _jobService = jobService;
            _candidateAccountService = candidateAccountService;
            _skillsService = skillsService;
            _resumeService = resumeService;
            _statusService = statusService;
            _candidateEducationService = candidateEducationService;
            _candidateExperienceService = candidateExperienceService;
            _educationInstitutionService = educationInstitutionService;
            _degreeService = degreeService;
            _companyService = companyService;
        }

        public JobApplicationResponseViewModel GetJobApplicaionById(Guid Id)
        {
            JobApplicationResponseViewModel response = new JobApplicationResponseViewModel();

            var application = _dbContext.JobApplications.Find(Id);
            if(application == null)
            {
                response.Status = 404;
                response.Message = "Application with this Id does not exist";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved job application.";
            response.Application = new JobApplicationViewModel(application);
            return response;
        }

        public AllJobApplicationResponseViewModel GetJobApplicationByCandidateId(Guid CandidateId)
        {
            AllJobApplicationResponseViewModel response = new AllJobApplicationResponseViewModel();

            var candidate = _dbContext.Candidates.Find(CandidateId);
            if(candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            List<JobApplication> applications = _dbContext.JobApplications.Where(item => item.CandidateId == CandidateId).ToList();

            List<JobApplicationViewModel> returnApplications = new List<JobApplicationViewModel>();
            foreach(var application in applications)
            {
                returnApplications.Add(new JobApplicationViewModel(application));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all job applications with this candidateId.";
            response.AllJobApplications = returnApplications;
            return response;
        }

        public AllJobApplicationResponseViewModel GetJobApplicationByJobId(Guid JobId)
        {
            AllJobApplicationResponseViewModel response = new AllJobApplicationResponseViewModel();

            var job = _dbContext.Jobs.Find(JobId);
            if (job == null)
            {
                response.Status = 404;
                response.Message = "Job with this Id does not exist";
                return response;
            }

            List<JobApplication> applications = _dbContext.JobApplications.Where(item => item.JobId == JobId).ToList();

            List<JobApplicationViewModel> returnApplications = new List<JobApplicationViewModel>();
            foreach (var application in applications)
            {
                returnApplications.Add(new JobApplicationViewModel(application));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all job applications with this candidateId.";
            response.AllJobApplications = returnApplications;
            return response;
        }

        public AllJobApplicationResponseViewModel GetJobApplicationByClosedJobId(Guid ClosedJobId)
        {
            AllJobApplicationResponseViewModel response = new AllJobApplicationResponseViewModel();

            var job = _dbContext.ClosedJobs.Find(ClosedJobId);
            if (job == null)
            {
                response.Status = 404;
                response.Message = "Job with this Id does not exist";
                return response;
            }

            List<JobApplication> applications = _dbContext.JobApplications.Where(item => item.ClosedJobId == ClosedJobId).ToList();

            List<JobApplicationViewModel> returnApplications = new List<JobApplicationViewModel>();
            foreach (var application in applications)
            {
                returnApplications.Add(new JobApplicationViewModel(application));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all applications for closed job with this candidateId.";
            response.AllJobApplications = returnApplications;
            return response;
        }

        public AllJobApplicationResponseViewModel GetClosedJobApplicationByCandidateId(Guid CandidateId)
        {
            AllJobApplicationResponseViewModel response = new AllJobApplicationResponseViewModel();

            var candidate = _dbContext.Candidates.Find(CandidateId);
            if (candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            List<ClosedJobApplication> applications = _dbContext.ClosedJobApplications.Where(item => item.CandidateId == CandidateId).ToList();

            List<JobApplicationViewModel> returnApplications = new List<JobApplicationViewModel>();
            foreach (var application in applications)
            {
                returnApplications.Add(new JobApplicationViewModel(application));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all job applications with this candidateId.";
            response.AllJobApplications = returnApplications;
            return response;
        }

        public async Task<JobApplicationResponseViewModel> Apply(AddJobApplication application, Guid CandidateId)
        {
            JobApplicationResponseViewModel response;

            var candidate = _dbContext.Candidates.Find(CandidateId);
            if(candidate == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist.";
                return response;
            }
           
            var job = _dbContext.Jobs.Find(application.JobId);
            if(job == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 404;
                response.Message = "Job with this id doesn't exist";
                return response;
            }

            JobApplication jobApplication = new JobApplication();
            {
                jobApplication.JobId = application.JobId;
                jobApplication.CandidateId = CandidateId;
                jobApplication.StatusId = _jobStatusService.GetInitialStatus();
            };

            await _dbContext.JobApplications.AddAsync(jobApplication);
            await _dbContext.SaveChangesAsync();

            if(jobApplication == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 500;
                response.Message = "Unable to apply to this job, please try again.";
                return response;
            }

            response = new JobApplicationResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully applied too the job.";
            response.Application = new JobApplicationViewModel(jobApplication);
            return response;
            
        }
       
        public async Task<JobApplicationResponseViewModel> ChangeJobApplicationStatus(Guid ApplicationId, int StatusId)
        {
            JobApplicationResponseViewModel response;

            var application = _dbContext.JobApplications.Find(ApplicationId);
            if(application == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 404;
                response.Message = "Application with this Id does not exist.";
                return response;
            }

            var status = _dbContext.Status.Find(StatusId);
            if(status == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 404;
                response.Message = "Status with this Id does not exist.";
                return response;
            }

            response = new JobApplicationResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully changed the status of application.";

            if (_jobStatusService.IsRejectedStatus(StatusId))
            {
                ClosedJobApplication closedApplication = new ClosedJobApplication(application);
                closedApplication.StatusId = StatusId;

                await _dbContext.ClosedJobApplications.AddAsync(closedApplication);

                _interviewService.ChangeInterviewApplicationToClosedApplication(ApplicationId, closedApplication.ClosedJobApplicationId);

                _dbContext.JobApplications.Remove(application);

                if(closedApplication == null)
                {
                    response = new JobApplicationResponseViewModel();
                    response.Status = 500;
                    response.Message = "Unable to update status, please try again.";
                    return response;
                }

                response.Application = new JobApplicationViewModel(closedApplication);
            }
            else 
            {
                application.StatusId = StatusId;
                _dbContext.JobApplications.Update(application);

                response.Application = new JobApplicationViewModel(application);
            }

            await _dbContext.SaveChangesAsync();
            return response;
        }

        public async Task<AllApplicantResponseViewModel> GetApplicantsByJobId(Guid JobId)
        {
            AllApplicantResponseViewModel response = new AllApplicantResponseViewModel();

            var applications = GetJobApplicationByJobId(JobId);
            if(applications.Status != 200)
            {
                response.Status = applications.Status;
                response.Message = applications.Message;
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all applicants.";
            response.Applicants = new List<ApplicantViewModel>();

            foreach (var item in applications.AllJobApplications)
            {
                List<CompleteEducationViewModel> education = new List<CompleteEducationViewModel>();
                var educations = _candidateEducationService.GetAllEducationOfACandidate((Guid)item.CandidateId);
                foreach (var element in educations.CandidateEducation)
                {
                    CompleteEducationViewModel edu = new CompleteEducationViewModel();

                    var institution = _educationInstitutionService.GetInstitution((Guid)element.InstitutionId);
                    var degree = _degreeService.GetDegree((Guid)element.DegreeId);

                    edu.Education = element;
                    edu.Institution = institution.EducationInstitution;
                    edu.Degree = degree.Degree;

                    education.Add(edu);
                }

                List<ExperienceWithCompanyViewModel> exp = new List<ExperienceWithCompanyViewModel>();
                var experiences = _candidateExperienceService.GetAllExperienceOfACandidate((Guid)item.CandidateId);
                foreach (var element in experiences.Experiences)
                {
                    ExperienceWithCompanyViewModel experience = new ExperienceWithCompanyViewModel();

                    var company = _companyService.GetCompanyById((Guid)element.CompanyId);

                    experience.Experience = element;
                    experience.Company = company.Company;

                    exp.Add(experience);
                }

                ApplicantViewModel applicant = new ApplicantViewModel();

                var candidate = _candidateAccountService.GetCandidateById((Guid)item.CandidateId);
                var skills = _skillsService.GetSkillsOfACandidate((Guid)item.CandidateId);
                var resume = _resumeService.GetResumeOfACandidate((Guid)item.CandidateId);
                var status = _statusService.GetStatusById((int)item.StatusId);

                if(candidate.Candidate != null)
                {
                    applicant.ApplicationId = item.ApplicationId;
                    applicant.CandidateId = (Guid)item.CandidateId;
                    applicant.JobId = (Guid)item.JobId;
                    applicant.Candidate = candidate.Candidate;
                    applicant.Skills = skills.Skills;
                    applicant.Resume = resume.Resume;
                    applicant.Status = status.StatusViewModel;
                    applicant.Education = education;
                    applicant.Experience = exp;

                    response.Applicants.Add(applicant);
                }
            }

            return response;
        }
    }
}

