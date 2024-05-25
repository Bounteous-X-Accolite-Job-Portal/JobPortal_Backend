using Bountous_X_Accolite_Job_Portal.Models.DesignationWhichHasPrivilegeViewModel.cs;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface IDesignationWithPrivilegeService
    {
        Task<AllPrivilegeResponseViewModel> GetAllPrivileges();
        Task<PrivilegeResponseViewModel> GetPrivilegeWithId(int PrivilegeId);
        Task<PrivilegeResponseViewModel> AddPrivilege(AddPrivilegeViewModel addPrivilege, Guid EmpId);
        Task<PrivilegeResponseViewModel> RemovePrivilege(int PrivilegeId);
        Task<PrivilegeResponseViewModel> GetPrivilegeByDesignationId(int DesignationId);
    }
}
