namespace Bountous_X_Accolite_Job_Portal.Models.DesignationViewModel
{
    public class DesignationViewModel
    {
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public Guid? EmpId { get; set; }

        public DesignationViewModel(Designation designation)
        {
            DesignationId = designation.DesignationId;
            DesignationName = designation.DesignationName;
            EmpId = designation.EmpId;
        }
    }
}
