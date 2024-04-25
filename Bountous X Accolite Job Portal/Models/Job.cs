using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class Job
    {
        [Key]
        public string JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobCategory { get; set; }
        public string JobLocation { get; set; }
        public string JobDescription { get; set; } 
        public string JobType { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime LastDate { get; set; }
    }
}
