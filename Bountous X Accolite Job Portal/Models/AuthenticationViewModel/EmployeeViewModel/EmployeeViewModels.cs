namespace Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel
{
    public class EmployeeViewModels
    {
        public Guid EmployeeId { get; set; }
        public string EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public int? DesignationId { get; set; }
        public bool Inactive { get; set; }

        public EmployeeViewModels(Employee employee)
        {
            EmployeeId = employee.EmployeeId;   
            EmpId = employee.EmpId;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            Email = employee.Email;
            Phone = employee.Phone;
            DesignationId = employee.DesignationId;
            Inactive = employee.Inactive;
        }
    }
}
