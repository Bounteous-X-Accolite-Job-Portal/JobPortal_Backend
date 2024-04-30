namespace Bountous_X_Accolite_Job_Portal.Models.DegreeViewModel.DegreeResponseViewModel
{
    public class DegreeViewModel
    {
        public Guid DegreeId { get; set; }
        public string DegreeName { get; set; }
        public int DurationInYears { get; set; }
        public Guid? EmpId { get; set; }

        public DegreeViewModel(Degree degree)
        {
            DegreeId = degree.DegreeId;
            DegreeName = degree.DegreeName;
            DurationInYears = degree.DurationInYears;
            EmpId = degree.EmpId;
        }
    }
}
