using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.JobPositionViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobPositionViewModel.JobPositionResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class JobPositionService : IJobPositionService
    {
        private readonly ApplicationDbContext _context;

        public JobPositionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobPositionResponseViewModel> AddJobPosition(CreateJobPositionViewModel jobPosition , Guid EmpId)
        {
            JobPosition newPosition = new JobPosition();
            newPosition.PositionCode = jobPosition.PositionCode;
            newPosition.PositionName = jobPosition.PositionName;
            newPosition.Description = jobPosition.Description;
            newPosition.CategoryId = jobPosition.CategoryId;
            newPosition.EmpId = EmpId;
            
            await _context.JobPosition.AddAsync(newPosition);
            await _context.SaveChangesAsync();

            JobPositionResponseViewModel response = new JobPositionResponseViewModel();
            if (newPosition == null)
            {
                response.Status = 500;
                response.Message = "Unable to Add New Position";
            }
            else
            {
                response.Status = 200;
                response.Message = "Successfully Added New Category !!";
                response.jobPosition = new JobPositionViewModel(newPosition);
            }
            
            return response;
        }

        public async Task<JobPositionResponseViewModel> DeleteJobPosition(Guid PositionId)
        {
            JobPositionResponseViewModel response = new JobPositionResponseViewModel();

            var position = _context.JobPosition.Find(PositionId);
            if(position!=null)
            {
                _context.JobPosition.Remove(position);
                await _context.SaveChangesAsync();
                response.Status = 200;
                response.Message = "Position Deleted Successfully !";
            }
            else
            {
                response.Status = 500;
                response.Message = "Unable to Delete Position !";
            }
            return response;
        }

        public AllJobPositionResponseViewModel GetAllJobPositions()
        {
            List<JobPosition> list = _context.JobPosition.ToList();

            List<JobPositionViewModel> jobPositions = new List<JobPositionViewModel>();
            foreach (JobPosition jobPosition in list)
                jobPositions.Add(new JobPositionViewModel(jobPosition));

            AllJobPositionResponseViewModel response = new AllJobPositionResponseViewModel();
            response.allJobPositions = jobPositions;
            response.Status = 200;
            response.Message = "Successfully reterived Positions";
            return response;
        }

        public JobPositionResponseViewModel GetJobPositionById(Guid PositionId)
        {
            JobPositionResponseViewModel response = new JobPositionResponseViewModel();
            var position = _context.JobPosition.Find(PositionId);
            if(position!=null)
            {
                response.Status = 200;
                response.Message = "Position Successfully Reterived";
                response.jobPosition = new JobPositionViewModel(position);
            }
            else
            {
                response.Status = 404;
                response.Message = "Unable to Find Position";
            }
            return response;
        }

        public async Task<JobPositionResponseViewModel> UpdateJobPosition(EditJobPositionViewModel jobPosition)
        {
            JobPositionResponseViewModel response = new JobPositionResponseViewModel();

            var dbposition = _context.JobPosition.Find(jobPosition.PositionId);
            if(dbposition!=null)
            {
                dbposition.PositionCode = jobPosition.PositionCode;
                dbposition.PositionName = jobPosition.PositionName;
                dbposition.Description = jobPosition.Description;
                dbposition.CategoryId = jobPosition.CategoryId;

                _context.JobPosition.Update(dbposition);
                await _context.SaveChangesAsync();

                response.Status = 200;
                response.Message = "Position Successfully Updated";
                response.jobPosition = new JobPositionViewModel(dbposition);
            }
            else
            {
                response.Status=404;
                response.Message = "Unable to Find Position ! ";

            }
            return response;
        }
    }
}
