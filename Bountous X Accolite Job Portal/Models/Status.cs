namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Status
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime? CreatedAt { get; set; }

        public Guid? EmpId { get; set; }
        public Employee? Employee { get; set; }  
    }
}
