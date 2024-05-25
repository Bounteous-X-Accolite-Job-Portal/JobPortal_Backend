using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationModels;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationModels.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IJobStatusService _jobStatusService;
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
        private readonly IDistributedCache _cache;
        public JobApplicationService(
            ApplicationDbContext applicationDbContext, 
            IJobStatusService jobStatusService, 
            IJobService jobService, 
            ICandidateAccountService candidateAccountService, 
            ISkillsService skillsService, 
            IResumeService resumeService,
            IJobStatusService statusService,
            ICandidateExperienceService candidateExperienceService,
            ICandidateEducationService candidateEducationService,
            IEducationInstitutionService educationInstitutionService,
            IDegreeService degreeService,
            ICompanyService companyService,
            IDistributedCache cache
        )
        {
            _dbContext = applicationDbContext;
            _jobStatusService = jobStatusService;
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
            _cache = cache;
        }

        public async Task<JobApplicationResponseViewModel> GetJobApplicaionById(Guid Id)
        {
            JobApplicationResponseViewModel response = new JobApplicationResponseViewModel();

            string key = $"getJobApplicationsById-{Id}";
            string? getJobApplicationsByIdFromCache = await _cache.GetStringAsync(key);

            JobApplication application;
            if (string.IsNullOrWhiteSpace(getJobApplicationsByIdFromCache))
            {
                application = _dbContext.JobApplications.Find(Id);
                if (application == null)
                {
                    response.Status = 404;
                    response.Message = "Application with this Id does not exist";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(application));
            }
            else
            {
                application = JsonSerializer.Deserialize<JobApplication>(getJobApplicationsByIdFromCache);
            }

            response.Status = 200;
            response.Message = "Successfully retrieved job application.";
            response.Application = new JobApplicationViewModel(application);
            return response;
        }

        public async Task<AllJobApplicationResponseViewModel> GetJobApplicationByCandidateId(Guid CandidateId)
        {
            AllJobApplicationResponseViewModel response = new AllJobApplicationResponseViewModel();

            var candidate = await _candidateAccountService.GetCandidateById(CandidateId);
            if(candidate.Candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            string key = $"getJobApplicationsByCandidateId-{CandidateId}";
            string? getJobApplicationsByCandidateIdFromCache = await _cache.GetStringAsync(key);

            List<JobApplication> applications;
            if (string.IsNullOrWhiteSpace(getJobApplicationsByCandidateIdFromCache))
            {
                applications = _dbContext.JobApplications.Where(item => item.CandidateId == CandidateId).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(applications));
            }
            else
            {
                applications = JsonSerializer.Deserialize<List<JobApplication>>(getJobApplicationsByCandidateIdFromCache);
            }

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

        public async Task<AllJobApplicationResponseViewModel> GetJobApplicationByJobId(Guid JobId)
        {
            AllJobApplicationResponseViewModel response = new AllJobApplicationResponseViewModel();

            var job = await _jobService.GetJobById(JobId);
            if (job.job == null)
            {
                response.Status = 404;
                response.Message = "Job with this Id does not exist";
                return response;
            }

            string key = $"getJobApplicationsByJobId-{JobId}";
            string? getJobApplicationsByJobIdFromCache = await _cache.GetStringAsync(key);

            List<JobApplication> applications;
            if (string.IsNullOrWhiteSpace(getJobApplicationsByJobIdFromCache))
            {
                applications = _dbContext.JobApplications.Where(item => item.JobId == JobId).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(applications));
            }
            else
            {
                applications = JsonSerializer.Deserialize<List<JobApplication>>(getJobApplicationsByJobIdFromCache);
            }

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

        public async Task<AllJobApplicationResponseViewModel> GetJobApplicationByClosedJobId(Guid ClosedJobId)
        {
            AllJobApplicationResponseViewModel response = new AllJobApplicationResponseViewModel();

            var job = await _jobService.GetClosedJobById(ClosedJobId);
            if (job.ClosedJob == null)
            {
                response.Status = 404;
                response.Message = "Job with this Id does not exist";
                return response;
            }

            string key = $"getJobApplicationsByClosedJobId-{ClosedJobId}";
            string? getJobApplicationsByClosedJobIdFromCache = await _cache.GetStringAsync(key);

            List<JobApplication> applications;
            if (string.IsNullOrWhiteSpace(getJobApplicationsByClosedJobIdFromCache))
            {
                applications = _dbContext.JobApplications.Where(item => item.ClosedJobId == ClosedJobId).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(applications));
            }
            else
            {
                applications = JsonSerializer.Deserialize<List<JobApplication>>(getJobApplicationsByClosedJobIdFromCache);
            }

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

        public async Task<AllJobApplicationResponseViewModel> GetClosedJobApplicationByCandidateId(Guid CandidateId)
        {
            AllJobApplicationResponseViewModel response = new AllJobApplicationResponseViewModel();

            var candidate = await _candidateAccountService.GetCandidateById(CandidateId);
            if (candidate.Candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            string key = $"getClosedJobApplicationsByCandidateId-{CandidateId}";
            string? getClosedJobApplicationsByCandidateIdFromCache = await _cache.GetStringAsync(key);

            List<ClosedJobApplication> applications;
            if (string.IsNullOrWhiteSpace(getClosedJobApplicationsByCandidateIdFromCache))
            {
                applications = _dbContext.ClosedJobApplications.Where(item => item.CandidateId == CandidateId).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(applications));
            }
            else
            {
                applications = JsonSerializer.Deserialize<List<ClosedJobApplication>>(getClosedJobApplicationsByCandidateIdFromCache);
            }

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

        public async Task<Boolean> IsCandidateApplicable(Guid JobId, Guid CandidateId)
        {
            AllJobApplicationResponseViewModel alljobsApplied = await GetJobApplicationByCandidateId(CandidateId);

            foreach (JobApplicationViewModel jobApplication in alljobsApplied.AllJobApplications)
            {
                if (jobApplication.JobId == JobId)
                    return false;
            }

            return true;
        }
        public async Task<JobApplicationResponseViewModel> Apply(AddJobApplication application, Guid CandidateId)
        {
            JobApplicationResponseViewModel response;

            var candidate = await _candidateAccountService.GetCandidateById(CandidateId);
            if(candidate.Candidate == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist.";
                return response;
            }

            var job = await _jobService.GetJobById(application.JobId);
            if(job.job == null)
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
                jobApplication.StatusId = await _jobStatusService.GetInitialApplicationStatus();
            };

            await _dbContext.JobApplications.AddAsync(jobApplication);
            await _dbContext.SaveChangesAsync();

            if (jobApplication == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 500;
                response.Message = "Unable to apply to this job, please try again.";
                return response;
            }

            await _cache.RemoveAsync($"allJobApplications");
            await _cache.RemoveAsync($"getJobApplicationsByCandidateId-{jobApplication.CandidateId}");
            await _cache.RemoveAsync($"getJobApplicationsByJobId-{jobApplication.JobId}");

            response = new JobApplicationResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully applied too the job.";
            response.Application = new JobApplicationViewModel(jobApplication);
            return response;
        }

        public async Task<JobApplicationResponseViewModel> ChangeJobApplicationStatus(Guid ApplicationId, int StatusId)
        {
            JobApplicationResponseViewModel response;

            var application = await GetJobApplicaionById(ApplicationId);
            if(application.Application == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 404;
                response.Message = "Application with this Id does not exist.";
                return response;
            }

            var status = await _jobStatusService.GetStatusById(StatusId);
            if(status.StatusViewModel == null)
            {
                response = new JobApplicationResponseViewModel();
                response.Status = 404;
                response.Message = "Status with this Id does not exist.";
                return response;
            }

            response = new JobApplicationResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully changed the status of application.";

            JobApplication app = new JobApplication();
            app.ApplicationId = application.Application.ApplicationId;
            app.CandidateId = application.Application.CandidateId;
            app.JobId = application.Application.JobId;
            app.AppliedOn = application.Application.AppliedOn;
            app.ClosedJobId = application.Application.ClosedJobId;
            app.StatusId = application.Application.StatusId;

            if (await _jobStatusService.IsRejectedStatus(StatusId))
            {
                ClosedJobApplication closedApplication = new ClosedJobApplication(app);
                closedApplication.StatusId = StatusId;

                await _dbContext.ClosedJobApplications.AddAsync(closedApplication);

                if (closedApplication == null)
                {
                    response = new JobApplicationResponseViewModel();
                    response.Status = 500;
                    response.Message = "Unable to update status, please try again.";
                    return response;
                }

                ChangeInterviewApplicationToClosedApplication(ApplicationId, closedApplication.ClosedJobApplicationId);
                _dbContext.JobApplications.Remove(app);

                response.Application = new JobApplicationViewModel(closedApplication);

                await _cache.RemoveAsync($"allClosedJobApplications");
                await _cache.RemoveAsync($"getClosedJobApplicationsByCandidateId-{application.Application.CandidateId}");
                await _cache.RemoveAsync($"getJobApplicationsByClosedJobId-{closedApplication.ClosedJobId}");
            }
            else
            {
                if (StatusId == (await _jobStatusService.getInitialSuccesstatus()))
                {
                    SuccessfulJobApplication successfulApplication = new SuccessfulJobApplication();
                    successfulApplication.ApplicationId = app.ApplicationId;
                    successfulApplication.CandidateId = app.CandidateId;
                    successfulApplication.JobId = app.JobId;
                    successfulApplication.ClosedJobId = app.ClosedJobId;

                    await _dbContext.AddAsync(successfulApplication);
                    await _cache.RemoveAsync($"allSuccessfulJobApplications");
                }

                app.StatusId = StatusId;
                _dbContext.JobApplications.Update(app);

                response.Application = new JobApplicationViewModel(app);
            }

            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"allJobApplications");
            await _cache.RemoveAsync($"getJobApplicationsById-{app.ApplicationId}");
            await _cache.RemoveAsync($"getJobApplicationsByCandidateId-{app.CandidateId}");
            await _cache.RemoveAsync($"getJobApplicationsByJobId-{app.JobId}");
            return response;
        }

        public async void ChangeInterviewApplicationToClosedApplication(Guid ApplicationId, Guid ClosedApplicationId)
        {
            string key = $"getAllInterviewsByApplicationId-{ApplicationId}";
            string? getAllInterviewsByApplicationIdFromCache = await _cache.GetStringAsync(key);

            List<Interview> interviews;
            if (string.IsNullOrWhiteSpace(getAllInterviewsByApplicationIdFromCache))
            {
                interviews = _dbContext.Interviews.Where(item => item.ApplicationId == ApplicationId).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(interviews));
            }
            else
            {
                interviews = JsonSerializer.Deserialize<List<Interview>>(getAllInterviewsByApplicationIdFromCache);
            }

            foreach (Interview interview in interviews)
            {
                interview.ApplicationId = null;
                interview.ClosedJobApplicationId = ClosedApplicationId;

                _dbContext.Interviews.Update(interview);

                await _cache.RemoveAsync($"getInterviewById-{interview.InterviewId}");
                await _cache.RemoveAsync($"getAllInterviewsByInterviewerId-{interview.InterViewerId}");
            }

            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"allInterviews");
            await _cache.RemoveAsync($"getAllInterviewsByApplicationId-{ApplicationId}");
        }

        public async Task<AllApplicantResponseViewModel> GetApplicantsByJobId(Guid JobId)
        {
            AllApplicantResponseViewModel response = new AllApplicantResponseViewModel();

            var applications = await GetJobApplicationByJobId(JobId);
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
                var educations = await _candidateEducationService.GetAllEducationOfACandidate((Guid)item.CandidateId);
                foreach (var element in educations.CandidateEducation)
                {
                    CompleteEducationViewModel edu = new CompleteEducationViewModel();

                    var institution = await _educationInstitutionService.GetInstitution((Guid)element.InstitutionId);
                    var degree = await _degreeService.GetDegree((Guid)element.DegreeId);

                    edu.Education = element;
                    edu.Institution = institution.EducationInstitution;
                    edu.Degree = degree.Degree;

                    education.Add(edu);
                }

                List<ExperienceWithCompanyViewModel> exp = new List<ExperienceWithCompanyViewModel>();
                var experiences = await _candidateExperienceService.GetAllExperienceOfACandidate((Guid)item.CandidateId);
                foreach (var element in experiences.Experiences)
                {
                    ExperienceWithCompanyViewModel experience = new ExperienceWithCompanyViewModel();

                    var company = await _companyService.GetCompanyById((Guid)element.CompanyId);

                    experience.Experience = element;
                    experience.Company = company.Company;

                    exp.Add(experience);
                }

                ApplicantViewModel applicant = new ApplicantViewModel();

                var candidate = await _candidateAccountService.GetCandidateById((Guid)item.CandidateId);
                var skills = await _skillsService.GetSkillsOfACandidate((Guid)item.CandidateId);
                var resume = await _resumeService.GetResumeOfACandidate((Guid)item.CandidateId);
                var status = await _statusService.GetStatusById((int)item.StatusId);

                if (candidate.Candidate != null)
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

        public async Task<AllJobResponseViewModel> GetJobsAppliedByCandidateId(Guid CandidateId)
        {
            AllJobResponseViewModel response = new AllJobResponseViewModel();
            List<JobViewModel> appliedjobList = new List<JobViewModel>();

            string key = $"allJobs";
            string? allJobsFromCache = await _cache.GetStringAsync(key);

            List<Job> jobs;
            if (string.IsNullOrWhiteSpace(allJobsFromCache))
            {
                jobs = _dbContext.Jobs.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(jobs));
            }
            else
            {
                jobs = JsonSerializer.Deserialize<List<Job>>(allJobsFromCache);
            }

            HashSet<Guid?> appliedJobsId = new HashSet<Guid?>();

            AllJobApplicationResponseViewModel candidateApplications = await GetJobApplicationByCandidateId(CandidateId);
            if (candidateApplications.Status != 200)
            {
                response.Status = 401;
                response.Message = "Error in Fetching Job Applications !!";
                return response;
            }

            foreach (JobApplicationViewModel application in candidateApplications.AllJobApplications)
                appliedJobsId.Add(application.JobId);

            foreach (Job job in jobs)
            {
                if (appliedJobsId.Contains(job.JobId))
                    appliedjobList.Add(new JobViewModel(job));
            }

            response.Status = 200;
            response.Message = "Successfully Reterived All Applied Jobs";
            response.allJobs = appliedjobList;
            return response;
        }

        public async Task<AllApplicantResponseViewModel> GetApplicantsByClosedJobId(Guid JobId)
        {
            AllApplicantResponseViewModel response = new AllApplicantResponseViewModel();

            var applications = await GetJobApplicationByClosedJobId(JobId);
            if (applications.Status != 200)
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
                var educations = await _candidateEducationService.GetAllEducationOfACandidate((Guid)item.CandidateId);
                foreach (var element in educations.CandidateEducation)
                {
                    CompleteEducationViewModel edu = new CompleteEducationViewModel();

                    var institution = await _educationInstitutionService.GetInstitution((Guid)element.InstitutionId);
                    var degree = await _degreeService.GetDegree((Guid)element.DegreeId);

                    edu.Education = element;
                    edu.Institution = institution.EducationInstitution;
                    edu.Degree = degree.Degree;

                    education.Add(edu);
                }

                List<ExperienceWithCompanyViewModel> exp = new List<ExperienceWithCompanyViewModel>();
                var experiences = await _candidateExperienceService.GetAllExperienceOfACandidate((Guid)item.CandidateId);
                foreach (var element in experiences.Experiences)
                {
                    ExperienceWithCompanyViewModel experience = new ExperienceWithCompanyViewModel();

                    var company = await _companyService.GetCompanyById((Guid)element.CompanyId);

                    experience.Experience = element;
                    experience.Company = company.Company;

                    exp.Add(experience);
                }

                ApplicantViewModel applicant = new ApplicantViewModel();

                var candidate = await _candidateAccountService.GetCandidateById((Guid)item.CandidateId);
                var skills = await _skillsService.GetSkillsOfACandidate((Guid)item.CandidateId);
                var resume = await _resumeService.GetResumeOfACandidate((Guid)item.CandidateId);
                var status = await _statusService.GetStatusById((int)item.StatusId);

                if (candidate.Candidate != null)
                {
                    applicant.ApplicationId = item.ApplicationId;
                    applicant.CandidateId = (Guid)item.CandidateId;
                    applicant.JobId = (Guid)item.ClosedJobId;
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

        public async Task<SuccessfulApplicationsResponseViewModel> GetAllApplicationsWithSuccess()
        {
            SuccessfulApplicationsResponseViewModel response = new SuccessfulApplicationsResponseViewModel();

            string key = $"allSuccessfulJobApplications";
            string? allSuccessfulJobApplicationsFromCache = await _cache.GetStringAsync(key);

            List<SuccessfulJobApplication> allSuccessfulJobApplications;
            if (string.IsNullOrWhiteSpace(allSuccessfulJobApplicationsFromCache))
            {
                allSuccessfulJobApplications = _dbContext.SuccessfulJobs.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(allSuccessfulJobApplications));
            }
            else
            {
                allSuccessfulJobApplications = JsonSerializer.Deserialize<List<SuccessfulJobApplication>>(allSuccessfulJobApplicationsFromCache);
            }

            List<SuccessfulApplicationViewModel> successfulApplications = new List<SuccessfulApplicationViewModel>();
            foreach (var application in allSuccessfulJobApplications)
            {
                if (!application.IsOfferLetterGenerated)
                {
                    successfulApplications.Add(new SuccessfulApplicationViewModel(application));
                }
            }

            response.Status = 200;
            response.Message = "Successfully fetched all successful application.";
            response.successfulJobApplication = successfulApplications;
            return response;
        }
    }
}