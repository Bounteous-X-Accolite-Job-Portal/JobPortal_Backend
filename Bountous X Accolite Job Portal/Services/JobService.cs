using System.Reflection.Emit;
using Bountous_X_Accolite_Job_Portal.Data;

using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels;
using Bountous_X_Accolite_Job_Portal.Models.JobViewModels.JobResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobService : IJob
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
            newJob.JobType = job.JobType;

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
                response.job = new DisplayJobResponseViewModel(newJob);
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
                dbjob.JobType = job.JobType;
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

        public AllJobResponseViewModel GetAllJobs()
        {
            List<Job> list = _context.Jobs.ToList();
            List<DisplayJobResponseViewModel> jobList = new List<DisplayJobResponseViewModel>();
            foreach (Job job in list) 
                jobList.Add(new DisplayJobResponseViewModel(job));

            AllJobResponseViewModel response = new AllJobResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully reterived Jobs";
            response.allJobs = jobList;
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
                response.job = new DisplayJobResponseViewModel(job);
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
