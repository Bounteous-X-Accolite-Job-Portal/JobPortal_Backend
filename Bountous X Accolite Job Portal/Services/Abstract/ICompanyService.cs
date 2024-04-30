using Bountous_X_Accolite_Job_Portal.Models.CompanyViewModel;
using Bountous_X_Accolite_Job_Portal.Models.CompanyViewModel.CompanyResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface ICompanyService
    {
        AllCompanyResponseViewModel GetAllCompanies();
        CompanyResponseViewModel GetCompanyById(Guid Id);
        Task<CompanyResponseViewModel> AddCompany(AddCompanyViewModel addCompany, Guid EmpId);
        Task<CompanyResponseViewModel> UpdateCompany(UpdateCompanyViewModel updateCompany);
        Task<CompanyResponseViewModel> RemoveCompany(Guid id);
    }
}
