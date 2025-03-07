﻿using Bountous_X_Accolite_Job_Portal.Models.JobPositionViewModel;
using Bountous_X_Accolite_Job_Portal.Models.JobPositionViewModel.JobPositionResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IJobPositionService
    {
        Task<AllJobPositionResponseViewModel> GetAllJobPositions();
        Task<JobPositionResponseViewModel> GetJobPositionById(Guid PositionId);
        Task<JobPositionResponseViewModel> AddJobPosition(CreateJobPositionViewModel jobPosition , Guid EmpId);
        Task<JobPositionResponseViewModel> UpdateJobPosition(EditJobPositionViewModel jobPosition);
        Task<JobPositionResponseViewModel> DeleteJobPosition(Guid PositionId);
    }
}
