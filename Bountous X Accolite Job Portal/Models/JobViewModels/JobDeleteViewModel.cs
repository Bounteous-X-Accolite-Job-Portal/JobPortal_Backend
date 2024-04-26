using System.ComponentModel.DataAnnotations;

namespace Bountous_X_Accolite_Job_Portal.Models.JobViewModels
{
    public class JobDeleteViewModel
    {
        [Required]
        public string JobTtile { get; set; }
        [Required]
        public string JobCategory { get; set; }

    }
}
