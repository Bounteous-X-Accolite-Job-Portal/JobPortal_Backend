using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.DesignationWhichHasPrivilegeViewModel.cs;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class DesignationWithPrivilegeService : IDesignationWithPrivilegeService
    {
        private readonly ApplicationDbContext _dbContext;
        public DesignationWithPrivilegeService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AllPrivilegeResponseViewModel GetAllPrivileges()
        {
            AllPrivilegeResponseViewModel response = new AllPrivilegeResponseViewModel();

            List<DesignationWhichHasPrivilege> allPrivileges = _dbContext.DesignationWhichHasPrivileges.Where(item => true).ToList();
            List<PrivilegeViewModel> privileges = new List<PrivilegeViewModel>();
            foreach (var privilege in allPrivileges)
            {
                privileges.Add(new PrivilegeViewModel(privilege));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all privileged designation.";
            response.AllPrivileges = privileges;
            return response;
        }

        public PrivilegeResponseViewModel GetPrivilegeWithId(int PrivilegeId)
        {
            PrivilegeResponseViewModel response = new PrivilegeResponseViewModel();

            var privilege = _dbContext.DesignationWhichHasPrivileges.Find(PrivilegeId);
            if(privilege == null)
            {
                response.Status = 404;
                response.Message = "Privilege with this Id does not exist.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved privilege with given Id.";
            response.DesignationWithPrivilege = new PrivilegeViewModel(privilege);
            return response;
        }

        public async Task<PrivilegeResponseViewModel> AddPrivilege(AddPrivilegeViewModel addPrivilege, Guid EmpId)
        {
            PrivilegeResponseViewModel response = new PrivilegeResponseViewModel();

            var privilege = _dbContext.DesignationWhichHasPrivileges.Where(item => item.DesignationId == addPrivilege.DesignationId).FirstOrDefault();
            if (privilege != null)
            {
                response.Status = 404;
                response.Message = "Privilege with this Id already exist.";
                return response;
            }

            DesignationWhichHasPrivilege hasPrivilege = new DesignationWhichHasPrivilege();
            hasPrivilege.DesignationId = addPrivilege.DesignationId;
            hasPrivilege.EmployeeId = EmpId;

            await _dbContext.DesignationWhichHasPrivileges.AddAsync(hasPrivilege);
            await _dbContext.SaveChangesAsync();

            if(hasPrivilege == null)
            {
                response.Status = 500;
                response.Message = "Something went wrong, please try again.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully added privilege.";
            response.DesignationWithPrivilege = new PrivilegeViewModel(hasPrivilege);
            return response;
        }

        public async Task<PrivilegeResponseViewModel> RemovePrivilege(int PrivilegeId)
        {
            PrivilegeResponseViewModel response = new PrivilegeResponseViewModel();

            var privilege = _dbContext.DesignationWhichHasPrivileges.Find(PrivilegeId);
            if (privilege == null)
            {
                response.Status = 404;
                response.Message = "Privilege with this Id does not exist.";
                return response;
            }

            _dbContext.DesignationWhichHasPrivileges.Remove(privilege);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully removed privilege with given Id.";
            response.DesignationWithPrivilege = new PrivilegeViewModel(privilege);
            return response;
        }
    }
}
