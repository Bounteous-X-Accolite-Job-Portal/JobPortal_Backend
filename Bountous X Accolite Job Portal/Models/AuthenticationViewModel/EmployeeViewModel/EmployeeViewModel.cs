﻿namespace Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.EmployeeViewModel
{
    public class EmployeeViewModel
    {
        public string EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public int DesignationId { get; set; }

        public EmployeeViewModel(Employee employee)
        {
            EmpId = employee.EmpId;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            Email = employee.Email;
            Phone = employee.Phone;
            DesignationId = employee.DesignationId;
        }
    }
}
