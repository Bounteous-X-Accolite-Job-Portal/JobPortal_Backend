using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class CandidateEducationService : ICandidateEducationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;
        private readonly ICandidateAccountService _candidateAccountService;
        private readonly IDegreeService _degreeService;
        private readonly IEducationInstitutionService _educationInstitutionService;
        public CandidateEducationService(ApplicationDbContext dbContext, IDistributedCache cache, ICandidateAccountService candidateAccountService, IDegreeService degreeService, IEducationInstitutionService educationInstitutionService)
        {
            _dbContext = dbContext;
            _cache = cache;
            _candidateAccountService = candidateAccountService;
            _degreeService = degreeService;
            _educationInstitutionService = educationInstitutionService;
        }

        public async Task<MultipleEducationResponseViewModel> GetAllEducationOfACandidate(Guid CandidateId)
        {
            MultipleEducationResponseViewModel response = new MultipleEducationResponseViewModel();

            var candidate = await _candidateAccountService.GetCandidateById(CandidateId);
            if (candidate.Candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            string key = $"allEducationOfACandidateByCandidateId-{CandidateId}";
            string? allEducationOfACandidateByCandidateIdFromCache = await _cache.GetStringAsync(key);

            List<CandidateEducation> allEducationsOfCandidate;
            if (string.IsNullOrWhiteSpace(allEducationOfACandidateByCandidateIdFromCache))
            {
                allEducationsOfCandidate = _dbContext.CandidateEducations.Where(item => item.CandidateId == CandidateId).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(allEducationsOfCandidate));
            }
            else
            {
                allEducationsOfCandidate = JsonSerializer.Deserialize<List<CandidateEducation>>(allEducationOfACandidateByCandidateIdFromCache);
            }

            List<CandidateEducationViewModel> educations = new List<CandidateEducationViewModel>();
            foreach(var item in allEducationsOfCandidate)
            {
                educations.Add(new CandidateEducationViewModel(item));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all educations by Given Candidate Id.";
            response.CandidateEducation = educations;
            return response;
        }

        public async Task<CandidateEducationResponseViewModel> GetEducationById(Guid Id)
        {
            CandidateEducationResponseViewModel response = new CandidateEducationResponseViewModel();

            string key = $"getEducationById-{Id}";
            string? getEducationByIdFromCache = await _cache.GetStringAsync(key);

            CandidateEducation education;
            if (string.IsNullOrWhiteSpace(getEducationByIdFromCache))
            {
                education = _dbContext.CandidateEducations.Find(Id);
                if (education == null)
                {
                    response.Status = 404;
                    response.Message = "Candidate Education with this Id does not exist";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(education));
            }
            else
            {
                education = JsonSerializer.Deserialize<CandidateEducation>(getEducationByIdFromCache);
            }
            
            response.Status = 200;
            response.Message = "Successfully retrieved candidate education with given Id.";
            response.CandidateEducation = new CandidateEducationViewModel(education);
            return response;
        }

        public async Task<CandidateEducationResponseViewModel> AddCandidateEducation(AddCandidateEducationViewModel addCandidateEducation, Guid CandidateId)
        {
            CandidateEducationResponseViewModel response = new CandidateEducationResponseViewModel();

            var degree = await _degreeService.GetDegree((Guid)addCandidateEducation.DegreeId);
            if(degree.Degree == null)
            {
                response.Status = 404;
                response.Message = "This degree does not exist in database.";
                return response;
            }

            var institution = await _educationInstitutionService.GetInstitution((Guid)addCandidateEducation.InstitutionId);
            if(institution.EducationInstitution == null)
            {
                response.Status = 404;
                response.Message = "This institution does not exist in database.";
                return response;
            }

            CandidateEducation educationToBeAdded = new CandidateEducation
            {
                InstitutionOrSchoolName = addCandidateEducation.InstitutionOrSchoolName,
                StartYear = addCandidateEducation.StartYear,
                EndYear = addCandidateEducation.EndYear,
                Grade = addCandidateEducation.Grade,
                CandidateId = CandidateId,
                InstitutionId = addCandidateEducation.InstitutionId,
                DegreeId = addCandidateEducation.DegreeId
            };

            await _dbContext.CandidateEducations.AddAsync(educationToBeAdded);
            await _dbContext.SaveChangesAsync();
            
            if(educationToBeAdded == null)
            {
                response.Status = 500;
                response.Message = "Unable to add education, please try again.";
                return response;
            }

            await _cache.RemoveAsync($"getAllCandidateEducationsByDegreeId-{educationToBeAdded.DegreeId}");
            await _cache.RemoveAsync($"getAllCandidateEducationsByInstitutionId-{educationToBeAdded.InstitutionId}");
            await _cache.RemoveAsync($"allEducationOfACandidateByCandidateId-{educationToBeAdded.CandidateId}");

            response.Status = 200;
            response.Message = "Successfully added candidate education.";
            response.CandidateEducation = new CandidateEducationViewModel(educationToBeAdded);
            return response;
        }

        public async Task<CandidateEducationResponseViewModel> UpdateCandidateEducation(UpdateCandidateEducationViewModel updateEducation)
        {
            CandidateEducationResponseViewModel response = new CandidateEducationResponseViewModel();

            string key = $"getEducationById-{updateEducation.EducationId}";
            string? getEducationByIdFromCache = await _cache.GetStringAsync(key);

            CandidateEducation education;
            if (string.IsNullOrWhiteSpace(getEducationByIdFromCache))
            {
                education = _dbContext.CandidateEducations.Find(updateEducation.EducationId);
                if (education == null)
                {
                    response.Status = 404;
                    response.Message = "The candidate education you are trying to update does not exist in database.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(education));
            }
            else
            {
                education = JsonSerializer.Deserialize<CandidateEducation>(getEducationByIdFromCache);
            }

            var degree = await _degreeService.GetDegree((Guid)updateEducation.DegreeId);
            if (degree.Degree == null)
            {
                response.Status = 404;
                response.Message = "This degree does not exist in database.";
                return response;
            }

            var institution = await _educationInstitutionService.GetInstitution((Guid)updateEducation.InstitutionId);
            if (institution.EducationInstitution == null)
            {
                response.Status = 404;
                response.Message = "This institution does not exist in database.";
                return response;
            }

            education.InstitutionOrSchoolName = updateEducation.InstitutionOrSchoolName;
            education.StartYear = updateEducation.StartYear;
            education.EndYear = updateEducation.EndYear;
            education.Grade = updateEducation.Grade;
            education.InstitutionId = updateEducation.InstitutionId;
            education.DegreeId = updateEducation.DegreeId;

            _dbContext.CandidateEducations.Update(education);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"getEducationById-{education.EducationId}");
            await _cache.RemoveAsync($"getAllCandidateEducationsByDegreeId-{education.DegreeId}");
            await _cache.RemoveAsync($"getAllCandidateEducationsByInstitutionId-{education.InstitutionId}");
            await _cache.RemoveAsync($"allEducationOfACandidateByCandidateId-{education.CandidateId}");

            response.Status = 200;
            response.Message = "Successfully updated that candidate education.";
            response.CandidateEducation = new CandidateEducationViewModel(education);
            return response;
        }

        public async Task<CandidateEducationResponseViewModel> RemoveEducation(Guid Id)
        {
            CandidateEducationResponseViewModel response = new CandidateEducationResponseViewModel();

            string key = $"getEducationById-{Id}";
            string? getEducationByIdFromCache = await _cache.GetStringAsync(key);

            CandidateEducation education;
            if (string.IsNullOrWhiteSpace(getEducationByIdFromCache))
            {
                education = _dbContext.CandidateEducations.Find(Id);
                if (education == null)
                {
                    response.Status = 404;
                    response.Message = "Candidate Education with this Id does not exist";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(education));
            }
            else
            {
                education = JsonSerializer.Deserialize<CandidateEducation>(getEducationByIdFromCache);
            }
            
            _dbContext.CandidateEducations.Remove(education);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"getEducationById-{Id}");
            await _cache.RemoveAsync($"getAllCandidateEducationsByDegreeId-{education.DegreeId}");
            await _cache.RemoveAsync($"getAllCandidateEducationsByInstitutionId-{education.InstitutionId}");
            await _cache.RemoveAsync($"allEducationOfACandidateByCandidateId-{education.CandidateId}");

            response.Status = 200;
            response.Message = "Successfully removed that degree.";
            response.CandidateEducation = new CandidateEducationViewModel(education);
            return response;
        }
    }
}
