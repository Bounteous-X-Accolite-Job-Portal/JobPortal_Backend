using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobTypeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobTypeViewModel.JobTypeResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobTypeService : IJobTypeService
    {
        private readonly ApplicationDbContext _context;

        public JobTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobTypeResponseViewModel> AddJobType(CreateJobTypeViewModel jobType, Guid EmpId)
        {
            JobType newJobType = new JobType(jobType);
            newJobType.EmpId = EmpId;

            await _context.JobType.AddAsync(newJobType);
            await _context.SaveChangesAsync();

            JobTypeResponseViewModel response = new JobTypeResponseViewModel();
            if(newJobType==null)
            {
                response.Status = 500;
                response.Message = "Unable to Add New Job Type !";
            }
            else
            {
                response.Status = 200;
                response.Message = "Successfully Added New Job Type !";
                response.jobType = new JobTypeViewModel(newJobType);
            }
            return response;
        }

        public async Task<JobTypeResponseViewModel> DeleteJobType(Guid Id)
        {
            JobTypeResponseViewModel response = new JobTypeResponseViewModel();
            var jobType = _context.JobType.Find(Id);

            if(jobType!=null)
            {
                _context.JobType.Remove(jobType);
                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = "Job Type Successfully Deleted !";
            }
            else
            {
                response.Status = 404;
                response.Message = "Unable to Delete Category !";
            }
            return response;
        }

        public AllJobTypeResponseViewModel GetAllJobTypes()
        {
            List<JobType> list= _context.JobType.ToList();
            List<JobTypeViewModel> jobTypelist = new List<JobTypeViewModel>();
            
            foreach (var jobType in list)
            {
                jobTypelist.Add(new JobTypeViewModel(jobType));
            }

            AllJobTypeResponseViewModel response = new AllJobTypeResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully reterived Job Types";
            response.allJobTypes = jobTypelist;

            return response;
        }

        public JobTypeResponseViewModel GetJobTypeById(Guid Id)
        {
            JobTypeResponseViewModel response = new JobTypeResponseViewModel();
            var jobType = _context.JobType.Find(Id);

            if (jobType != null)
            {
                response.Status = 200;
                response.Message = "Job Type Successfully Reterived !";
                response.jobType = new JobTypeViewModel(jobType);
            }
            else
            {
                response.Status = 404;
                response.Message = "Unable to Find Job Type !";
            }
            return response;
        }

        public async Task<JobTypeResponseViewModel> UpdateJobType(EditJobTypeViewModel jobType)
        {
            JobTypeResponseViewModel response = new JobTypeResponseViewModel();
            var dbjobType = _context.JobType.Find(jobType.JobTypeId);

            if (dbjobType != null)
            {
                dbjobType.TypeName = jobType.TypeName;

                response.Status = 200;
                response.Message = "Job Type Successfully Updated !";

                response.jobType = new JobTypeViewModel(dbjobType);
            }
            else
            {
                response.Status = 404;
                response.Message = "Unable to Update Job Type !";
            }
            return response;
        }
    }
}
