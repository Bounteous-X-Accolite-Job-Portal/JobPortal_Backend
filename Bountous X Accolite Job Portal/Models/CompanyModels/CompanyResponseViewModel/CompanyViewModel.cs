namespace Bountous_X_Accolite_Job_Portal.Models.CompanyModels.CompanyResponseViewModel
{
    public class CompanyViewModel
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string BaseUrl { get; set; }
        public string CompanyDescription { get; set; }
        public Guid? EmpId { get; set; }

        public CompanyViewModel(Company company)
        {
            CompanyId = company.CompanyId;
            CompanyName = company.CompanyName;
            BaseUrl = company.BaseUrl;
            CompanyDescription = company.CompanyDescription;
            EmpId = company.EmpId;
        }
    }
}
