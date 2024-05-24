using Bountous_X_Accolite_Job_Portal.Models.CompanyModels;
using Bountous_X_Accolite_Job_Portal.Models.CompanyModels.CompanyResponseViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface ICompanyService
    {
        Task<AllCompanyResponseViewModel> GetAllCompanies();
        Task<CompanyResponseViewModel> GetCompanyById(Guid Id);
        Task<CompanyResponseViewModel> AddCompany(AddCompanyViewModel addCompany, Guid EmpId);
        Task<CompanyResponseViewModel> UpdateCompany(UpdateCompanyViewModel updateCompany);
        Task<CompanyResponseViewModel> RemoveCompany(Guid id);
    }
}
