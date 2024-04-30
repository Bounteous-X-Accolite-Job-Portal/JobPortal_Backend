using System.ComponentModel.DataAnnotations;
using Bountous_X_Accolite_Job_Portal.Models.JobTypeViewModel;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class JobType
    {
        [Key]
        public Guid JobTypeId { get; set; }
        public string TypeName { get; set; }
        public Guid? EmpId { get; set; }
        public virtual Employee? Employee { get; set; }  

        public JobType() { }
        public JobType(CreateJobTypeViewModel job) 
        {
            TypeName = job.TypeName;
        }

    }
}
