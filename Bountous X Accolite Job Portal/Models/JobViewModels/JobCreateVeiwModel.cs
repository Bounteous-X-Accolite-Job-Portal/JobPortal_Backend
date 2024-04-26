using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models.JobViewModels
{
    public class JobCreateVeiwModel
    {
        [Required]
        public string JobTtile {  get; set; }
        [Required]
        public string JobCategory {  get; set; }
        [Required]
        public string JobLocation { get; set; }
        public string JobDescription { get; set; }
        public string JobType { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime LastDate { get; set; }
        public string qualifiicaton { get; set; }
        public string Experience { get; set; }

    }
}
