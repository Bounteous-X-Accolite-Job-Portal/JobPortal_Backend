using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.SkillsModels;
using Bountous_X_Accolite_Job_Portal.Models.SkillsModels.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class SkillsService : ISkillsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;
        private readonly ICandidateAccountService _candidateAccountService;
        public SkillsService(ApplicationDbContext dbContext, IDistributedCache cache, ICandidateAccountService candidateAccountService)
        {
            _dbContext = dbContext;
            _cache = cache;
            _candidateAccountService = candidateAccountService;
        }
        public async Task<SkillsResponseViewModel> GetSkillsById(Guid Id)
        {
            SkillsResponseViewModel response = new SkillsResponseViewModel();

            string key = $"getSkillsById-{Id}";
            string? getSkillsByIdFromCache = await _cache.GetStringAsync(key);

            Skills skills;
            if (string.IsNullOrEmpty(getSkillsByIdFromCache))
            {
                skills = _dbContext.Skills.Find(Id);
                if (skills == null)
                {
                    response.Status = 404;
                    response.Message = "Skills with this Id does not exist";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(skills));
            }
            else
            {
                skills = JsonSerializer.Deserialize<Skills>(getSkillsByIdFromCache);
            }

            response.Status = 200;
            response.Message = "Successfully retrieved skills with given Id.";
            response.Skills = new SkillsViewModel(skills);
            return response;
        }

        public async Task<SkillsResponseViewModel> GetSkillsOfACandidate(Guid CandidateId)
        {
            SkillsResponseViewModel response = new SkillsResponseViewModel();

            var candidate = await _candidateAccountService.GetCandidateById(CandidateId);
            if (candidate.Candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            string key = $"getSkillsByCandidateId-{CandidateId}";
            string? getSkillsByCandidateIdFromCache = await _cache.GetStringAsync(key);

            Skills skills;
            if (string.IsNullOrEmpty(getSkillsByCandidateIdFromCache))
            {
                skills = _dbContext.Skills.Where(item => item.CandidateId == CandidateId).FirstOrDefault();
                if (skills == null)
                {
                    response.Status = 404;
                    response.Message = "Skills of this candidate does not exist.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(skills));
            }
            else
            {
                skills = JsonSerializer.Deserialize<Skills>(getSkillsByCandidateIdFromCache);
            }

            response.Status = 200;
            response.Message = "Successfully retrieved the social media details of given candidate.";
            response.Skills = new SkillsViewModel(skills);
            return response;
        }

        public async Task<SkillsResponseViewModel> AddSkills(AddSkillsViewModel addSkills, Guid CandidateId)
        {
            Skills skills = new Skills
            {
                CandidateId = CandidateId,
                CandidateSkills = addSkills.CandidateSkills
            };

            await _dbContext.Skills.AddAsync(skills);
            await _dbContext.SaveChangesAsync();

            SkillsResponseViewModel response = new SkillsResponseViewModel();
            if (skills == null)
            {
                response.Status = 500;
                response.Message = "Unable to add skills, please try again.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully added candidate resume.";
            response.Skills = new SkillsViewModel(skills);
            return response;
        }

        public async Task<SkillsResponseViewModel> UpdateSkills(UpdateSkillsViewModel updateSkills, Guid candidateId)
        {
            SkillsResponseViewModel response = new SkillsResponseViewModel();

            string key = $"getSkillsById-{updateSkills.SkillsId}";
            string? getSkillsByIdFromCache = await _cache.GetStringAsync(key);

            Skills skills;
            if (string.IsNullOrEmpty(getSkillsByIdFromCache))
            {
                skills = _dbContext.Skills.Find(updateSkills.SkillsId);
                if (skills == null)
                {
                    response.Status = 404;
                    response.Message = "Skills with this Id does not exist";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(skills));
            }
            else
            {
                skills = JsonSerializer.Deserialize<Skills>(getSkillsByIdFromCache);
            }

            if(skills.CandidateId != candidateId)
            {
                response.Status = 401;
                response.Message = "You are not authorised to update these skills.";
                return response;
            }

            skills.CandidateSkills = updateSkills.CandidateSkills;

            _dbContext.Skills.Update(skills);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"getSkillsById-{updateSkills.SkillsId}");
            await _cache.RemoveAsync($"getSkillsByCandidateId-{skills.CandidateId}");

            response.Status = 200;
            response.Message = "Successfully updated that candidate experience.";
            response.Skills = new SkillsViewModel(skills);
            return response;
        }
    }
}
