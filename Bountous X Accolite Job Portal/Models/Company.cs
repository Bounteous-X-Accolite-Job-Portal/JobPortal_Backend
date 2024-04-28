using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Company
    {
        [Key]
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string BaseUrl { get; set; }
        public string CompanyDescription { get; set; }  
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid? EmpId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
