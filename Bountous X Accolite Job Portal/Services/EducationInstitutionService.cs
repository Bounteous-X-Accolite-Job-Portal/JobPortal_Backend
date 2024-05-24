using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.EducationInstitutionViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class EducationInstitutionService : IEducationInstitutionService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;
        public EducationInstitutionService(ApplicationDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<List<InstitutionViewModel>> GetAllInstitutions()
        {
            string key = $"allInstitutions";
            string? getAllInstitutionsFromCache = await _cache.GetStringAsync(key);

            List<EducationInstitution> list;
            if (string.IsNullOrEmpty(getAllInstitutionsFromCache))
            {
                list = _dbContext.EducationInstitutions.Where(item => true).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(list));
            }
            else
            {
                list = JsonSerializer.Deserialize<List<EducationInstitution>>(getAllInstitutionsFromCache);
            }

            List<InstitutionViewModel> response = new List<InstitutionViewModel>();
            foreach (var item in list)
            {
                response.Add(new InstitutionViewModel(item));
            }

            return response;
        }

        public async Task<InstitutionResponseViewModel> GetInstitution(Guid id)
        {
            InstitutionResponseViewModel response = new InstitutionResponseViewModel();

            string key = $"getInstitutionById-{id}";
            string? getInstitutionByIdFromCache = await _cache.GetStringAsync(key);

            EducationInstitution institution;
            if (string.IsNullOrEmpty(getInstitutionByIdFromCache))
            {
                institution = _dbContext.EducationInstitutions.Find(id);
                if (institution == null)
                {
                    response.Status = 404;
                    response.Message = "Institution with this Id does not exist";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(institution));
            }
            else
            {
                institution = JsonSerializer.Deserialize<EducationInstitution>(getInstitutionByIdFromCache);
            }
            
            response.Status = 200;
            response.Message = "Successfully retrieved institution with given Id.";
            response.EducationInstitution = new InstitutionViewModel(institution);
            return response;
        }

        public async Task<InstitutionResponseViewModel> AddInstitution(AddInstitutionViewModel institution, Guid EmpId)
        {
            EducationInstitution institutionToBeAdded = new EducationInstitution();
            institutionToBeAdded.EmpId = EmpId;
            institutionToBeAdded.InstitutionOrSchool = institution.InstitutionOrSchool;
            institutionToBeAdded.UniversityOrBoard = institution.UniversityOrBoard;

            await _dbContext.EducationInstitutions.AddAsync(institutionToBeAdded);
            await _dbContext.SaveChangesAsync();

            InstitutionResponseViewModel response = new InstitutionResponseViewModel();
            if(institutionToBeAdded == null)
            {
                response.Status = 500;
                response.Message = "Unable to add institution, please try again.";
                return response;
            }

            await _cache.RemoveAsync($"allInstitutions");

            response.Status = 200;
            response.Message = "Successfully added institution.";
            response.EducationInstitution = new InstitutionViewModel(institutionToBeAdded);
            return response;
        }

        public async Task<InstitutionResponseViewModel> UpdateInstution(UpdateInstitutionViewModel updateInstitution)
        {
            InstitutionResponseViewModel response = new InstitutionResponseViewModel();

            string key = $"getInstitutionById-{updateInstitution.InstitutionId}";
            string? getInstitutionByIdFromCache = await _cache.GetStringAsync(key);

            EducationInstitution institution;
            if (string.IsNullOrEmpty(getInstitutionByIdFromCache))
            {
                institution = _dbContext.EducationInstitutions.Find(updateInstitution.InstitutionId);
                if (institution == null)
                {
                    response.Status = 404;
                    response.Message = "The institution you are trying to update does not exist in database.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(institution));
            }
            else
            {
                institution = JsonSerializer.Deserialize<EducationInstitution>(getInstitutionByIdFromCache);
            }

            institution.InstitutionOrSchool = updateInstitution.InstitutionOrSchool;
            institution.UniversityOrBoard = updateInstitution.UniversityOrBoard;

            _dbContext.EducationInstitutions.Update(institution);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"allInstitutions");
            await _cache.RemoveAsync($"getInstitutionById-{institution.InstitutionId}");

            response.Status = 200;
            response.Message = "Successfully updated that institution.";
            response.EducationInstitution = new InstitutionViewModel(institution);
            return response;
        }

        public async Task<InstitutionResponseViewModel> RemoveInstitution(Guid id)
        {
            InstitutionResponseViewModel res = new InstitutionResponseViewModel();

            string key = $"getInstitutionById-{id}";
            string? getInstitutionByIdFromCache = await _cache.GetStringAsync(key);

            EducationInstitution institution;
            if (string.IsNullOrEmpty(getInstitutionByIdFromCache))
            {
                institution = _dbContext.EducationInstitutions.Find(id);
                if (institution == null)
                {
                    res.Status = 404;
                    res.Message = "Institution with this Id does not exist";
                    return res; // data with that Id does not exist
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(institution));
            }
            else
            {
                institution = JsonSerializer.Deserialize<EducationInstitution>(getInstitutionByIdFromCache);
            }

            key = $"getAllCandidateEducationsByInstitutionId-{id}";
            string? getAllCandidateEducationsByInstitutionIdFromCache = await _cache.GetStringAsync(key);

            List<CandidateEducation> education;
            if (string.IsNullOrEmpty(getAllCandidateEducationsByInstitutionIdFromCache))
            {
                education = _dbContext.CandidateEducations.Where(item => item.InstitutionId == id).ToList();
                if (education.Count != 0)
                {
                    res.Status = 409;
                    res.Message = "Candidate Education Section still using this institution.";
                    return res;  // conflict
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(education));
            }
            else
            {
                education = JsonSerializer.Deserialize<List<CandidateEducation>>(getAllCandidateEducationsByInstitutionIdFromCache);
            }

            _dbContext.EducationInstitutions.Remove(institution);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"allInstitutions");
            await _cache.RemoveAsync($"getInstitutionById-{id}");

            res.Status = 200;
            res.Message = "Successfully removed that institution.";
            res.EducationInstitution = new InstitutionViewModel(institution);
            return res;
        }
    }
}
