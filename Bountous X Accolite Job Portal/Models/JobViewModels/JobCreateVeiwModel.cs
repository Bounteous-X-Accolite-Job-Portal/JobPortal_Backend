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
        [Required]
        public string JobDescription { get; set; }
        [Required]
        public string JobType { get; set; }
        [Required]
        public DateTime PostDate { get; set; }
        [Required]
        public DateTime LastDate { get; set; }
        [Required]
        public string qualifiicaton { get; set; }
        [Required]
        public string Experience { get; set; }

    }
}
