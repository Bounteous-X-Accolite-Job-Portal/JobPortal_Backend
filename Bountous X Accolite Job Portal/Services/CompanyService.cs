using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Bountous_X_Accolite_Job_Portal.Models.CompanyModels.CompanyResponseViewModel;
using Bountous_X_Accolite_Job_Portal.Models.CompanyModels;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;
        public CompanyService(ApplicationDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache; 
        }

        public async Task<AllCompanyResponseViewModel> GetAllCompanies()
        {
            string key = $"allCompanies";
            string? getAllCompaniesFromCache = await _cache.GetStringAsync(key);

            List<Company> list;
            if (string.IsNullOrEmpty(getAllCompaniesFromCache))
            {
                list = _dbContext.Company.Where(item => true).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<Company>>(getAllCompaniesFromCache);
            }

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

        public async Task<CompanyResponseViewModel> GetCompanyById(Guid id)
        {
            CompanyResponseViewModel response = new CompanyResponseViewModel();

            string key = $"getCompanyById-{id}";
            string? getCompanyByIdFromCache = await _cache.GetStringAsync(key);

            Company company;
            if (string.IsNullOrEmpty(getCompanyByIdFromCache))
            {
                company = _dbContext.Company.Find(id);
                if (company == null)
                {
                    response.Status = 404;
                    response.Message = "Company with this Id does not exist";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(company));
            }
            else
            {
                company = JsonSerializer.Deserialize<Company>(getCompanyByIdFromCache);
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

            await _cache.RemoveAsync($"allCompanies");

            response.Status = 200;
            response.Message = "Successfully added Company.";
            response.Company = new CompanyViewModel(companyToBeAdded);
            return response;
        }

        public async Task<CompanyResponseViewModel> UpdateCompany(UpdateCompanyViewModel updateCompany)
        {
            CompanyResponseViewModel response = new CompanyResponseViewModel();

            string key = $"getCompanyById-{updateCompany.CompanyId}";
            string? getCompanyByIdFromCache = await _cache.GetStringAsync(key);

            Company company;
            if (string.IsNullOrEmpty(getCompanyByIdFromCache))
            {
                company = _dbContext.Company.Find(updateCompany.CompanyId);
                if (company == null)
                {
                    response.Status = 404;
                    response.Message = "The Company you are trying to update does not exist in database.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(company));
            }
            else
            {
                company = JsonSerializer.Deserialize<Company>(getCompanyByIdFromCache);
            }

            company.CompanyName = updateCompany.CompanyName;
            company.BaseUrl = updateCompany.BaseUrl;
            company.CompanyDescription = updateCompany.CompanyDescription;

            _dbContext.Company.Update(company);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"allCompanies");
            await _cache.RemoveAsync($"getCompanyById-{updateCompany.CompanyId}");
            await _cache.RemoveAsync($"getAllCandidateExperiencesByCompanyId-{updateCompany.CompanyId}");

            response.Status = 200;
            response.Message = "Successfully updated that Company.";
            response.Company = new CompanyViewModel(company);
            return response;
        }

        public async Task<CompanyResponseViewModel> RemoveCompany(Guid id)
        {
            CompanyResponseViewModel response = new CompanyResponseViewModel();

            string key = $"getCompanyById-{id}";
            string? getCompanyByIdFromCache = await _cache.GetStringAsync(key);

            Company company;
            if (string.IsNullOrEmpty(getCompanyByIdFromCache))
            {
                company = _dbContext.Company.Find(id);
                if (company == null)
                {
                    response.Status = 404;
                    response.Message = "Company with this Id does not exist";
                    return response; // data with that Id does not exist
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(company));
            }
            else
            {
                company = JsonSerializer.Deserialize<Company>(getCompanyByIdFromCache);
            }

            key = $"getAllCandidateExperiencesByCompanyId-{id}";
            string? getAllCandidateExperiencesByCompanyIdFromCache = await _cache.GetStringAsync(key);

            List<CandidateExperience> experience;
            if (string.IsNullOrEmpty(getAllCandidateExperiencesByCompanyIdFromCache))
            {
                experience = _dbContext.CandidateExperience.Where(item => item.CompanyId == id).ToList();
                if (experience.Count != 0)
                {
                    response.Status = 409;
                    response.Message = "Candidate Experience Section still using this company.";
                    return response;  // conflict
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(experience));
            }
            else
            {
                experience = JsonSerializer.Deserialize<List<CandidateExperience>>(getAllCandidateExperiencesByCompanyIdFromCache);
            }

            _dbContext.Company.Remove(company);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"allCompanies");
            await _cache.RemoveAsync($"getCompanyById-{id}");
            await _cache.RemoveAsync($"getAllCandidateExperiencesByCompanyId-{id}");

            response.Status = 200;
            response.Message = "Successfully removed that Company.";
            response.Company = new CompanyViewModel(company);
            return response;
        }
    }
}
