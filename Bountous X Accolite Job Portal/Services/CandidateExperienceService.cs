using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel;
using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class CandidateExperienceService : ICandidateExperienceService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;
        private readonly ICandidateAccountService _candidateAccountService;
        private readonly ICompanyService _companyService;
        public CandidateExperienceService(ApplicationDbContext dbContext, IDistributedCache cache, ICandidateAccountService candidateAccountService, ICompanyService companyService)
        {
            _dbContext = dbContext;
            _cache = cache;
            _candidateAccountService = candidateAccountService;
            _companyService = companyService;
        }

        public async Task<MultipleExperienceResponseViewModel> GetAllExperienceOfACandidate(Guid CandidateId)
        {
            MultipleExperienceResponseViewModel response = new MultipleExperienceResponseViewModel();

            var candidate = await _candidateAccountService.GetCandidateById(CandidateId);
            if (candidate.Candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            string key = $"getAllCandidateExperiencesByCandidateId-{CandidateId}";
            string? getAllCandidateExperiencesByCandidateIdFromCache = await _cache.GetStringAsync(key);

            List<CandidateExperience> allExperienceOfCandidate;
            if (string.IsNullOrWhiteSpace(getAllCandidateExperiencesByCandidateIdFromCache))
            {
                allExperienceOfCandidate = _dbContext.CandidateExperience.Where(item => item.CandidateId == CandidateId).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(allExperienceOfCandidate));
            }
            else
            {
                allExperienceOfCandidate = JsonSerializer.Deserialize<List<CandidateExperience>>(getAllCandidateExperiencesByCandidateIdFromCache);
            }

            List<CandidateExperienceViewModel> experiences = new List<CandidateExperienceViewModel>();
            foreach (var item in allExperienceOfCandidate)
            {
                experiences.Add(new CandidateExperienceViewModel(item));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all experience by Given Candidate Id.";
            response.Experiences = experiences;
            return response;
        }

        public async Task<CandidateExperienceResponseViewModel> GetExperienceById(Guid Id)
        {
            CandidateExperienceResponseViewModel response = new CandidateExperienceResponseViewModel();

            string key = $"getCandidateExperienceById-{Id}";
            string? getCandidateExperienceByIdFromCache = await _cache.GetStringAsync(key);

            CandidateExperience experience;
            if (string.IsNullOrWhiteSpace(getCandidateExperienceByIdFromCache))
            {
                experience = _dbContext.CandidateExperience.Find(Id);
                if (experience == null)
                {
                    response.Status = 404;
                    response.Message = "Candidate Experience with this Id does not exist";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(experience));
            }
            else
            {
                experience = JsonSerializer.Deserialize<CandidateExperience>(getCandidateExperienceByIdFromCache);
            }
            
            response.Status = 200;
            response.Message = "Successfully retrieved candidate experience with given Id.";
            response.Experience = new CandidateExperienceViewModel(experience);
            return response;
        }

        public async Task<CandidateExperienceResponseViewModel> AddCandidateExperience(AddCandidateExperienceViewModel addExperience, Guid CandidateId)
        {
            CandidateExperienceResponseViewModel response = new CandidateExperienceResponseViewModel();

            var company = await _companyService.GetCompanyById((Guid)addExperience.CompanyId);
            if (company.Company == null)
            {
                response.Status = 404;
                response.Message = "This company does not exist in database.";
                return response;
            }

            CandidateExperience experienceToBeAdded = new CandidateExperience
            {
                ExperienceTitle = addExperience.ExperienceTitle,
                StartDate = addExperience.StartDate,
                EndDate = addExperience.EndDate,
                Description = addExperience.Description,
                IsCurrentlyWorking = addExperience.IsCurrentlyWorking,
                CompanyId = addExperience.CompanyId,
                CandidateId = CandidateId
            };

            await _dbContext.CandidateExperience.AddAsync(experienceToBeAdded);
            await _dbContext.SaveChangesAsync();

            if (experienceToBeAdded == null)
            {
                response.Status = 500;
                response.Message = "Unable to add experience, please try again.";
                return response;
            }

            await _cache.RemoveAsync($"getAllCandidateExperiencesByCandidateId-{experienceToBeAdded.CandidateId}");
            await _cache.RemoveAsync($"getAllCandidateExperiencesByCompanyId-{experienceToBeAdded.CompanyId}");

            response.Status = 200;
            response.Message = "Successfully added candidate experience.";
            response.Experience = new CandidateExperienceViewModel(experienceToBeAdded);
            return response;
        }

        public async Task<CandidateExperienceResponseViewModel> UpdateCandidateExperience(UpdateCandidateExperienceViewModel updateExperience)
        {
            CandidateExperienceResponseViewModel response = new CandidateExperienceResponseViewModel();

            string key = $"getCandidateExperienceById-{updateExperience.ExperienceId}";
            string? getCandidateExperienceByIdFromCache = await _cache.GetStringAsync(key);

            CandidateExperience experience;
            if (string.IsNullOrWhiteSpace(getCandidateExperienceByIdFromCache))
            {
                experience = _dbContext.CandidateExperience.Find(updateExperience.ExperienceId);
                if (experience == null)
                {
                    response.Status = 404;
                    response.Message = "The candidate experience you are trying to update does not exist in database.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(experience));
            }
            else
            {
                experience = JsonSerializer.Deserialize<CandidateExperience>(getCandidateExperienceByIdFromCache);
            }

            var company = await _companyService.GetCompanyById((Guid)updateExperience.CompanyId);
            if (company.Company == null)
            {
                response.Status = 404;
                response.Message = "This company does not exist in database.";
                return response;
            }

            experience.ExperienceTitle = updateExperience.ExperienceTitle;
            experience.StartDate = updateExperience.StartDate;
            experience.EndDate = updateExperience.EndDate;
            experience.IsCurrentlyWorking = updateExperience.IsCurrentlyWorking;
            experience.Description = updateExperience.Description;
            experience.CompanyId = updateExperience.CompanyId;

            _dbContext.CandidateExperience.Update(experience);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"getCandidateExperienceById-{experience.ExperienceId}");
            await _cache.RemoveAsync($"getAllCandidateExperiencesByCandidateId-{experience.CandidateId}");
            await _cache.RemoveAsync($"getAllCandidateExperiencesByCompanyId-{experience.CompanyId}");

            response.Status = 200;
            response.Message = "Successfully updated that candidate experience.";
            response.Experience = new CandidateExperienceViewModel(experience);
            return response;
        }

        public async Task<CandidateExperienceResponseViewModel> RemoveExperience(Guid Id)
        {
            CandidateExperienceResponseViewModel response = new CandidateExperienceResponseViewModel();

            string key = $"getCandidateExperienceById-{Id}";
            string? getCandidateExperienceByIdFromCache = await _cache.GetStringAsync(key);

            CandidateExperience experience;
            if (string.IsNullOrWhiteSpace(getCandidateExperienceByIdFromCache))
            {
                experience = _dbContext.CandidateExperience.Find(Id);
                if (experience == null)
                {
                    response.Status = 404;
                    response.Message = "The candidate experience you are trying to remove does not exist in database.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(experience));
            }
            else
            {
                experience = JsonSerializer.Deserialize<CandidateExperience>(getCandidateExperienceByIdFromCache);
            }

            _dbContext.CandidateExperience.Remove(experience);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"getCandidateExperienceById-{Id}");
            await _cache.RemoveAsync($"getAllCandidateExperiencesByCandidateId-{experience.CandidateId}");
            await _cache.RemoveAsync($"getAllCandidateExperiencesByCompanyId-{experience.CompanyId}");

            response.Status = 200;
            response.Message = "Successfully removed that experience.";
            response.Experience = new CandidateExperienceViewModel(experience);
            return response;
        }
    }
}
