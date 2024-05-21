namespace Bountous_X_Accolite_Job_Portal.Models.DesignationWhichHasPrivilegeViewModel.cs
{
    public class PrivilegeViewModel
    {
        public int PrivilegeId { get; set; }
        public int? DesignationId { get; set; }
        public Guid? EmployeeId { get; set; }

        public PrivilegeViewModel(DesignationWhichHasPrivilege privilege)
        {
            PrivilegeId = privilege.PrivilegeId;
            DesignationId = privilege.DesignationId;
            EmployeeId = privilege.EmployeeId;
        }
    }
}
