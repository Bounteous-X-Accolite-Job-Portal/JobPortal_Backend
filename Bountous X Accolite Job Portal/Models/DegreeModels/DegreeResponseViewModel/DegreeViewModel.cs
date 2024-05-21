namespace Bountous_X_Accolite_Job_Portal.Models.DegreeModels.DegreeResponseViewModel
{
    public class DegreeViewModel
    {
        public Guid DegreeId { get; set; }
        public string DegreeName { get; set; }
        public int DurationInYears { get; set; }

        public DegreeViewModel(Degree degree)
        {
            DegreeId = degree.DegreeId;
            DegreeName = degree.DegreeName;
            DurationInYears = degree.DurationInYears;
        }
    }
}
