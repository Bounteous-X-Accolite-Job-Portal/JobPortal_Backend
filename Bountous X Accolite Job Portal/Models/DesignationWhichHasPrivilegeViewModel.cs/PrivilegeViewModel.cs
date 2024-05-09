namespace Bountous_X_Accolite_Job_Portal.Models.DesignationWhichHasPrivilegeViewModel.cs
{
    public class PrivilegeViewModel
    {
        public int? DesignationId { get; set; }
        public Guid? EmployeeId { get; set; }

        public PrivilegeViewModel(DesignationWhichHasPrivilege privilege)
        {
            DesignationId = privilege.DesignationId;
            EmployeeId = privilege.EmployeeId;
        }
    }
}
