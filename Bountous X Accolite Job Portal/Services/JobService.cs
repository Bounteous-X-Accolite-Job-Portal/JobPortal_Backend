using Bountous_X_Accolite_Job_Portal.Data;

using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.ClosedJobViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobService : IJobService
    {
        private readonly ApplicationDbContext _context;

        public JobService(ApplicationDbContext context)
        {
            _context = context;
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
                response.Status = 200;
                response.Message = "Successfully Added New Job !!";
                response.job = new JobViewModel(newJob);
            }
            return response;
        }

        public async Task<JobResponseViewModel> DeleteJob(Guid JobId)
        {
            JobResponseViewModel response = new JobResponseViewModel();
            var job = _context.Jobs.Find(JobId);
            if(job != null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
                response.Status = 200;
                response.Message = "Job Successfully Deleted !";
            }
            else
            {
                response.Status = 500;
                response.Message = "Unable to Delete Job !";
            }
            return response;
        }

        public async Task<JobResponseViewModel> EditJob(EditJobViewModel job)
        {
            JobResponseViewModel response = new JobResponseViewModel();
            var dbjob = _context.Jobs.Find(job.JobId);
            if (dbjob != null)
            {
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
                
                response.Status = 200;
                response.Message = "Job Successfully Updated !";
            }
            else
            {
                response.Status = 404;
                response.Message = "Unable to Update Job !";
            }
            return response;
        }

        public async Task<AllJobResponseViewModel> GetAllJobs()
        {
            List<Job> list = _context.Jobs.ToList();
            List<JobViewModel> jobList = new List<JobViewModel>();

            Dictionary<Guid, Job> dic = new Dictionary<Guid, Job>();
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

            List<JobApplication> application = _context.JobApplications.Where(item => true).ToList();

            List<JobApplication> validApplications = new List<JobApplication>();
            foreach (JobApplication app in application)
            {
                if (dic.ContainsKey((Guid)app.JobId))
                {
                    validApplications.Add(app);
                }
            }

            Dictionary<Guid, Guid> closedDic = new Dictionary<Guid, Guid>();
            foreach (KeyValuePair<Guid, Job> entry in dic)
            {
                // do something with entry.Value or entry.Key
                ClosedJob closedJob = new ClosedJob(entry.Value);
                await _context.ClosedJobs.AddAsync(closedJob);

                closedDic.Add(entry.Key, closedJob.ClosedJobId);
            }

            foreach(JobApplication app in validApplications)
            {
                app.ClosedJobId = closedDic[(Guid)app.JobId];
                app.JobId = null;
                _context.JobApplications.Update(app);
            }

            foreach (KeyValuePair<Guid, Job> entry in dic)
            {
                _context.Jobs.Remove(dic[(Guid)entry.Key]);
            }

            await _context.SaveChangesAsync();

            AllJobResponseViewModel response = new AllJobResponseViewModel();
            response.Status = 200;
            response.allJobs = jobList;
            if(jobList.Count>0)
                response.Message = "Successfully reterived Jobs";
            else
                response.Message = "No Published Jobs Found";

            return response;
        }

        public AllJobResponseViewModel GetAllJobsByEmployeeId(Guid EmpId)
        {
            List<Job> list = _context.Jobs.Where(e => e.EmployeeId==EmpId).ToList() ;
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

        public JobResponseViewModel GetJobById(Guid jobId)
        {
            JobResponseViewModel response = new JobResponseViewModel();
            var job = _context.Jobs.Find(jobId);
            if (job != null)
            {
                response.Status = 200;
                response.Message = "Job Successfully Found !";
                response.job = new JobViewModel(job);
            }
            else
            {
                response.Status = 500;
                response.Message = "Unable to Find Job !";
            }
            return response;
        }

        public AllClosedJobResponseViewModel GetAllClosedJobsByEmployeeId(Guid EmpId)
        {
            List<ClosedJob> list = _context.ClosedJobs.Where(e => e.EmployeeId == EmpId).ToList();
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

        public ClosedJobResponseViewModel GetClosedJobById(Guid jobId)
        {
            ClosedJobResponseViewModel response = new ClosedJobResponseViewModel();
            var job = _context.ClosedJobs.Find(jobId);
            if (job != null)
            {
                response.Status = 200;
                response.Message = "Job Successfully Found !";
                response.ClosedJob = new ClosedJobViewModel(job);
            }
            else
            {
                response.Status = 500;
                response.Message = "Unable to Find Job !";
            }
            return response;
        }
    }
}
