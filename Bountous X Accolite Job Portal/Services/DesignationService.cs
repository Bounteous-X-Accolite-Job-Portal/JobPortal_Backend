using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class DesignationService : IDesignationService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public DesignationService(UserManager<User> userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _dbContext = applicationDbContext;
        }

        public async Task<DesignationResponseViewModel> AddDesignation(AddDesignationViewModel designation, Guid empId)
        {
            DesignationResponseViewModel response = new DesignationResponseViewModel();

            Designation addDesignation = new Designation();
            addDesignation.DesignationName = designation.DesignationName;
            addDesignation.EmpId = empId;

            await _dbContext.Designations.AddAsync(addDesignation);    
            await _dbContext.SaveChangesAsync();

            if(addDesignation == null)
            {
                response.Status = 500;
                response.Message = "Something went wrong, plaese try again.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully add designation.";
            return response;
        }
    }
}
