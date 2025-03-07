﻿using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.ClosedJobViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection.Metadata;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobService : IJobService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;
        private readonly IReferralService _referralService;
        public JobService(ApplicationDbContext context, IDistributedCache cache, IReferralService referralService)
        {
            _context = context;
            _cache = cache;
            _referralService = referralService;
        }

        public async Task<AllClosedJobResponseViewModel> GetAllClosedJobs()
        {
            string key = $"allClosedJobs";
            string? allClosedJobsFromCache = await _cache.GetStringAsync(key);

            List<ClosedJob> list;
            if (string.IsNullOrWhiteSpace(allClosedJobsFromCache))
            {
                list = _context.ClosedJobs.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<ClosedJob>>(allClosedJobsFromCache);
            }

            List<ClosedJobViewModel> closedJobs = new List<ClosedJobViewModel>();
            foreach (var item in list)
            {
                closedJobs.Add(new ClosedJobViewModel(item));
            }

            AllClosedJobResponseViewModel response = new AllClosedJobResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully retrived all closed jobs";
            response.ClosedJobs = closedJobs;
            return response;
        }

        public async Task<JobResponseViewModel> AddJob(CreateJobViewModel job, Guid EmpId)
        {
            Job newJob = new Job();
            newJob.EmployeeId = EmpId;
            newJob.LocationId = job.LocationId;
            newJob.PositionId = job.PositionId;
            newJob.CategoryId = job.CategoryId;
            newJob.DegreeId = job.DegreeId;

            newJob.JobCode = job.JobCode;
            newJob.JobTitle = job.JobTitle;
            newJob.JobDescription = job.JobDescription;
            newJob.JobTypeId = job.JobType;

            newJob.LastDate = job.LastDate;
            newJob.Experience = job.Experience;

            await _context.Jobs.AddAsync(newJob);
            await _context.SaveChangesAsync();
            
            JobResponseViewModel response = new JobResponseViewModel();
            if (newJob == null)
            {
                response.Status = 500;
                response.Message = "Unable to Add New Job";
            }
            else
            {
                await _cache.RemoveAsync($"allJobs");
                await _cache.RemoveAsync($"getAllJobsByEmployeeId-{EmpId}");

                response.Status = 200;
                response.Message = "Successfully Added New Job !!";
                response.job = new JobViewModel(newJob);
            }
            return response;
        }

        public async Task<JobResponseViewModel> DeleteJob(Guid JobId)
        {
            JobResponseViewModel response = new JobResponseViewModel();

            string key = $"getJobById-{JobId}";
            string? getJobByIdFromCache = await _cache.GetStringAsync(key);

            Job? job;
            if (string.IsNullOrWhiteSpace(getJobByIdFromCache))
            {
                job = _context.Jobs.Find(JobId);
                if (job == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Find Job !";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(job));
            }
            else
            {
                job = JsonSerializer.Deserialize<Job>(getJobByIdFromCache);
            }

            key = $"getJobApplicationsByJobId-{JobId}";
            string? getJobApplicationsByJobIdFromCache = await _cache.GetStringAsync(key);

            List<JobApplication> applications;
            if (string.IsNullOrWhiteSpace(getJobApplicationsByJobIdFromCache))
            {
                applications = _context.JobApplications.Where(item => item.JobId == JobId).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(applications));
            }
            else
            {
                applications = JsonSerializer.Deserialize<List<JobApplication>>(getJobApplicationsByJobIdFromCache);
            }

            if(applications.Count != 0)
            {
                response.Status = 400;
                response.Message = "Job Application with this jobId still exist.";
                return response;
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"allJobs");
            await _cache.RemoveAsync($"getAllJobsByEmployeeId-{job.EmployeeId}");
            await _cache.RemoveAsync($"getJobById-{JobId}");
            await _cache.RemoveAsync($"getJobApplicationsByJobId-{JobId}");

            response.Status = 200;
            response.Message = "Job Successfully Deleted !";
            return response;
        }

        public async Task<JobResponseViewModel> EditJob(EditJobViewModel job)
        {
            JobResponseViewModel response = new JobResponseViewModel();

            string key = $"getJobById-{job.JobId}";
            string? getJobByIdFromCache = await _cache.GetStringAsync(key);

            Job? dbjob;
            if (string.IsNullOrWhiteSpace(getJobByIdFromCache))
            {
                dbjob = _context.Jobs.Find(job.JobId);
                if (dbjob == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Find Job !";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(dbjob));
            }
            else
            {
                dbjob = JsonSerializer.Deserialize<Job>(getJobByIdFromCache);
            }

            dbjob.JobCode = job.JobCode;
            dbjob.JobTitle = job.JobTitle;
            dbjob.JobDescription = job.JobDescription;
            dbjob.LastDate = job.LastDate;
            dbjob.JobTypeId = job.JobType;
            dbjob.LocationId = job.LocationId;
            dbjob.DegreeId = job.DegreeId;
            dbjob.Experience = job.Experience;
            dbjob.CategoryId = job.CategoryId;
            dbjob.PositionId = job.PositionId;

            _context.Jobs.Update(dbjob);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"allJobs");
            await _cache.RemoveAsync($"getAllJobsByEmployeeId-{dbjob.EmployeeId}");
            await _cache.RemoveAsync($"getJobById-{job.JobId}");

            response.Status = 200;
            response.Message = "Job Successfully Updated !";
            response.job = new JobViewModel(dbjob);
            return response;
        }

        public async Task<AllJobResponseViewModel> GetAllJobs()
        {
            string key = $"allJobs";
            string? allJobsFromCache = await _cache.GetStringAsync(key);

            List<Job> list;
            if (string.IsNullOrWhiteSpace(allJobsFromCache))
            {
                list = _context.Jobs.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<Job>>(allJobsFromCache);
            }

            Dictionary<Guid, Job> dic = new Dictionary<Guid, Job>();
            List<JobViewModel> jobList = new List<JobViewModel>();
            foreach (Job job in list)
            {
                if(job.LastDate <= DateTime.Now)
                {
                    dic.Add(job.JobId, job);
                }
                else
                {
                    jobList.Add(new JobViewModel(job));
                }
            }

            if (dic.Count > 0)
            {
                key = $"allJobApplications";
                string? allJobApplicationsFromCache = await _cache.GetStringAsync(key);

                List<JobApplication> application;
                if (string.IsNullOrWhiteSpace(allJobApplicationsFromCache))
                {
                    application = _context.JobApplications.Where(item => true).ToList();
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize(application));
                }
                else
                {
                    application = JsonSerializer.Deserialize<List<JobApplication>>(allJobApplicationsFromCache);
                }

                List<JobApplication> validApplications = new List<JobApplication>();
                foreach (JobApplication app in application)
                {
                    if (app.JobId != null && dic.ContainsKey((Guid)app.JobId))
                    {
                        validApplications.Add(app);
                    }
                }

                if(validApplications.Count > 0)
                {
                    await _cache.RemoveAsync($"allJobApplications");
                }

                key = $"allClosedJobApplications";
                string? allClosedJobApplicationsFromCache = await _cache.GetStringAsync(key);

                List<ClosedJobApplication> closedApplication;
                if (string.IsNullOrWhiteSpace(allClosedJobApplicationsFromCache))
                {
                    closedApplication = _context.ClosedJobApplications.Where(item => true).ToList();
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize(closedApplication));
                }
                else
                {
                    closedApplication = JsonSerializer.Deserialize<List<ClosedJobApplication>>(allClosedJobApplicationsFromCache);
                }

                List<ClosedJobApplication> validClosedApplications = new List<ClosedJobApplication>();
                foreach (ClosedJobApplication app in closedApplication)
                {
                    if (app.JobId != null && dic.ContainsKey((Guid)app.JobId))
                    {
                        validClosedApplications.Add(app);
                    }
                }

                if( validClosedApplications.Count > 0)
                {
                    await _cache.RemoveAsync($"allClosedJobApplications");
                }

                // Add ClosedApplication Id to referral
                key = $"getAllReferrals";
                string? getAllReferralsFromCache = await _cache.GetStringAsync(key);

                List<Referral> referrals;
                if (string.IsNullOrWhiteSpace(getAllReferralsFromCache))
                {
                    referrals = _context.Referrals.ToList();
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize(referrals));
                }
                else
                {
                    referrals = JsonSerializer.Deserialize<List<Referral>>(getAllReferralsFromCache);
                }

                Dictionary<Guid, List<Referral>> referal = new Dictionary<Guid, List<Referral>>();
                foreach (var item in referrals)
                {
                    if (item.ClosedJobId == null)
                    {
                        if (!referal.ContainsKey((Guid)item.JobId))
                        {
                            referal.Add((Guid)item.JobId, new List<Referral>());
                        }
                        referal[(Guid)item.JobId].Add(item);
                    }
                }

                // Add ClosedApplication Id to successfull job applications
                key = $"allSuccessfulJobApplications";
                string? allSuccessfulJobApplicationsFromCache = await _cache.GetStringAsync(key);

                List<SuccessfulJobApplication> allSuccessfulJobApplications;
                if (string.IsNullOrWhiteSpace(allSuccessfulJobApplicationsFromCache))
                {
                    allSuccessfulJobApplications = _context.SuccessfulJobs.ToList();
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize(allSuccessfulJobApplications));
                }
                else
                {
                    allSuccessfulJobApplications = JsonSerializer.Deserialize<List<SuccessfulJobApplication>>(allSuccessfulJobApplicationsFromCache);
                }

                if(allSuccessfulJobApplications.Count > 0)
                {
                    await _cache.RemoveAsync($"allSuccessfulJobApplications");
                }

                Dictionary<Guid, List<SuccessfulJobApplication>> successfulJobApplication = new Dictionary<Guid, List<SuccessfulJobApplication>>();
                foreach (var item in allSuccessfulJobApplications)
                {
                    if (item.ClosedJobId == null)
                    {
                        if (!successfulJobApplication.ContainsKey((Guid)item.JobId))
                        {
                            successfulJobApplication.Add((Guid)item.JobId, new List<SuccessfulJobApplication>());
                        }
                        successfulJobApplication[(Guid)item.JobId].Add(item);
                    }
                }

                Dictionary<Guid, Guid> closedDic = new Dictionary<Guid, Guid>();
                foreach (KeyValuePair<Guid, Job> entry in dic)
                {
                    // do something with entry.Value or entry.Key
                    ClosedJob closedJob = new ClosedJob(entry.Value);
                    await _context.ClosedJobs.AddAsync(closedJob);

                    closedDic.Add(entry.Key, closedJob.ClosedJobId);

                    if (referal.TryGetValue(entry.Key, out List<Referral>? value))
                    {
                        var refe = value;
                        foreach (var item in refe)
                        {
                            await _referralService.AddClosedJobIdToReferral(item, closedJob.ClosedJobId);
                        }
                    }

                    if (successfulJobApplication.ContainsKey(entry.Key))
                    {
                        var successApplications = successfulJobApplication[entry.Key];
                        foreach (var item in successApplications)
                        {
                            item.JobId = null;
                            item.ClosedJobId = closedJob.ClosedJobId;

                            _context.SuccessfulJobs.Update(item);
                        }
                    }

                    await _cache.RemoveAsync($"getAllClosedJobsByEmployeeId-{entry.Value.EmployeeId}");
                }

                foreach (JobApplication app in validApplications)
                {
                    app.ClosedJobId = closedDic[(Guid)app.JobId];
                    app.JobId = null;
                    _context.JobApplications.Update(app);

                    await _cache.RemoveAsync($"getJobApplicationsById-{app.ApplicationId}");
                    await _cache.RemoveAsync($"getJobApplicationsByCandidateId-{app.CandidateId}");
                    await _cache.RemoveAsync($"getJobApplicationsByJobId-{app.JobId}");
                }

                foreach (ClosedJobApplication app in validClosedApplications)
                {
                    app.ClosedJobId = closedDic[(Guid)app.JobId];
                    app.JobId = null;
                    _context.ClosedJobApplications.Update(app);

                    await _cache.RemoveAsync($"getJobApplicationsByClosedJobId-{app.ClosedJobId}");
                    await _cache.RemoveAsync($"getClosedJobApplicationsByCandidateId-{app.CandidateId}");
                }

                foreach (KeyValuePair<Guid, Job> entry in dic)
                {
                    await _cache.RemoveAsync($"getJobById-{entry.Key}");
                    await _cache.RemoveAsync($"getAllJobsByEmployeeId-{entry.Value.EmployeeId}");
                    _context.Jobs.Remove(entry.Value);
                }

                await _cache.RemoveAsync($"allJobs");
                await _cache.RemoveAsync($"allClosedJobs");

                await _context.SaveChangesAsync();
            }

            AllJobResponseViewModel response = new AllJobResponseViewModel();

            response.Status = 200;
            response.allJobs = jobList;

            if(jobList.Count>0)
                response.Message = "Successfully reterived Jobs";
            else
                response.Message = "No Published Jobs Found";

            return response;
        }

        public async Task<AllJobResponseViewModel> GetAllJobsByEmployeeId(Guid EmpId)
        {
            string key = $"getAllJobsByEmployeeId-{EmpId}";
            string? allJobsByEmployeeIdFromCache = await _cache.GetStringAsync(key);

            List<Job> list;
            if (string.IsNullOrWhiteSpace(allJobsByEmployeeIdFromCache))
            {
                list = _context.Jobs.Where(e => e.EmployeeId == EmpId).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<Job>>(allJobsByEmployeeIdFromCache);
            }

            List<JobViewModel> jobList = new List<JobViewModel>();
            foreach (Job job in list)
                jobList.Add(new JobViewModel(job));

            AllJobResponseViewModel response = new AllJobResponseViewModel();
            response.Status = 200;
            response.allJobs = jobList;
            if(list.Count>0)
                response.Message = "Successfully reterived Jobs for Given Employee !";
            else
                response.Message = "No Jobs Published By Employee !";

            return response;
        }

        public async Task<JobResponseViewModel> GetJobById(Guid jobId)
        {
            JobResponseViewModel response = new JobResponseViewModel();

            string key = $"getJobById-{jobId}";
            string? getJobByIdFromCache = await _cache.GetStringAsync(key);

            Job? job;
            if (string.IsNullOrWhiteSpace(getJobByIdFromCache))
            {
                job = _context.Jobs.Find(jobId);
                if(job == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Find Job !";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(job));
            }
            else
            {
                job = JsonSerializer.Deserialize<Job>(getJobByIdFromCache);
            }

            response.Status = 200;
            response.Message = "Job Successfully Found !";
            response.job = new JobViewModel(job);
            return response;
        }

        public async Task<AllClosedJobResponseViewModel> GetAllClosedJobsByEmployeeId(Guid EmpId)
        {
            string key = $"getAllClosedJobsByEmployeeId-{EmpId}";
            string? allClosedJobsByEmployeeId = await _cache.GetStringAsync(key);

            List<ClosedJob> list;
            if (string.IsNullOrWhiteSpace(allClosedJobsByEmployeeId))
            {
                list = _context.ClosedJobs.Where(e => e.EmployeeId == EmpId).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<ClosedJob>>(allClosedJobsByEmployeeId);
            }

            List<ClosedJobViewModel> jobList = new List<ClosedJobViewModel>();
            foreach (ClosedJob job in list)
                jobList.Add(new ClosedJobViewModel(job));

            AllClosedJobResponseViewModel response = new AllClosedJobResponseViewModel();
            response.Status = 200;
            response.ClosedJobs = jobList;
            if (list.Count > 0)
                response.Message = "Successfully reterived Jobs for Given Employee !";
            else
                response.Message = "No Jobs Published By Employee !";

            return response;
        }

        public async Task<ClosedJobResponseViewModel> GetClosedJobById(Guid jobId)
        {
            ClosedJobResponseViewModel response = new ClosedJobResponseViewModel();

            string key = $"getClosedJobById-{jobId}";
            string? getClosedJobByIdFromCache = await _cache.GetStringAsync(key);

            ClosedJob? closedJob;
            if (string.IsNullOrWhiteSpace(getClosedJobByIdFromCache))
            {
                closedJob = _context.ClosedJobs.Find(jobId);
                if (closedJob == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Find Closed Job !";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(closedJob));
            }
            else
            {
                closedJob = JsonSerializer.Deserialize<ClosedJob>(getClosedJobByIdFromCache);
            }

            response.Status = 200;
            response.Message = "Job Successfully Found !";
            response.ClosedJob = new ClosedJobViewModel(closedJob);
            return response;
        }

        public async Task<JobResponseViewModel> DisableJob(Guid jobId)
        {
            JobResponseViewModel response = new JobResponseViewModel();

            string key = $"getJobById-{jobId}";
            string? getJobByIdFromCache = await _cache.GetStringAsync(key);

            Job? job;
            if (string.IsNullOrWhiteSpace(getJobByIdFromCache))
            {
                job = _context.Jobs.Find(jobId);
                if (job == null)
                {
                    response.Status = 404;
                    response.Message = "Unable to Find Job !";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(job));
            }
            else
            {
                job = JsonSerializer.Deserialize<Job>(getJobByIdFromCache);
            }

            job.LastDate = DateTime.Now;

            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"allJobs");
            await _cache.RemoveAsync($"getAllJobsByEmployeeId-{job.EmployeeId}");
            await _cache.RemoveAsync($"getJobById-{jobId}");

            response.job = new JobViewModel(job);
            response.Status = 200;
            response.Message = "Successfully Disabled Job !!";

            return response;
        }
    }
}
