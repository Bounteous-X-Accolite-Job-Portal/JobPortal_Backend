using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Bountous_X_Accolite_Job_Portal.Models.CompanyViewModel.CompanyResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Models.CompanyViewModel;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _dbContext;
        public CompanyService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AllCompanyResponseViewModel GetAllCompanies()
        {
            List<Company> list = _dbContext.Company.Where(item => true).ToList();

            List<CompanyViewModel> companyList = new List<CompanyViewModel>();
            foreach (var item in list)
            {
                companyList.Add(new CompanyViewModel(item));
            }

            AllCompanyResponseViewModel response = new AllCompanyResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully retrieved all companies.";
            response.Companies = companyList;
            return response;
        }

        public CompanyResponseViewModel GetCompanyById(Guid id)
        {
            CompanyResponseViewModel response = new CompanyResponseViewModel();

            var company = _dbContext.Company.Find(id);
            if (company == null)
            {
                response.Status = 404;
                response.Message = "Company with this Id does not exist";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved company with given Id.";
            response.Company = new CompanyViewModel(company);
            return response;
        }

        public async Task<CompanyResponseViewModel> AddCompany(AddCompanyViewModel addCompany, Guid EmpId)
        {
            Company companyToBeAdded = new Company
            {
                EmpId = EmpId,
                CompanyName = addCompany.CompanyName,
                BaseUrl = addCompany.BaseUrl,
                CompanyDescription = addCompany.CompanyDescription
            };

            await _dbContext.Company.AddAsync(companyToBeAdded);
            await _dbContext.SaveChangesAsync();

            CompanyResponseViewModel response = new CompanyResponseViewModel();
            if (companyToBeAdded == null)
            {
                response.Status = 500;
                response.Message = "Unable to add company, please try again.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully added Company.";
            response.Company = new CompanyViewModel(companyToBeAdded);
            return response;
        }

        public async Task<CompanyResponseViewModel> UpdateCompany(UpdateCompanyViewModel updateCompany)
        {
            CompanyResponseViewModel response = new CompanyResponseViewModel();

            var company = _dbContext.Company.Find(updateCompany.CompanyId);
            if (company == null)
            {
                response.Status = 404;
                response.Message = "The Company you are trying to update does not exist in database.";
                return response;
            }

            company.CompanyName = updateCompany.CompanyName;
            company.BaseUrl = updateCompany.BaseUrl;
            company.CompanyDescription = updateCompany.CompanyDescription;

            _dbContext.Company.Update(company);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully updated that Company.";
            response.Company = new CompanyViewModel(company);
            return response;
        }

        public async Task<CompanyResponseViewModel> RemoveCompany(Guid id)
        {
            CompanyResponseViewModel response = new CompanyResponseViewModel();

            var company = _dbContext.Company.Find(id);
            if (company == null)
            {
                response.Status = 404;
                response.Message = "Company with this Id does not exist";
                return response; // data with that Id does not exist
            }

            var experience = _dbContext.CandidateExperience.Where(item => item.CompanyId == id).ToList();
            if (experience.Count != 0)
            {
                response.Status = 409;
                response.Message = "Candidate Experience Section still using this company.";
                return response;  // conflict
            }

            _dbContext.Company.Remove(company);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully removed that Company.";
            response.Company = new CompanyViewModel(company);
            return response;
        }
    }
}
