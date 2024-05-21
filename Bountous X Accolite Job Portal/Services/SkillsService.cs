using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.SkillsModels;
using Bountous_X_Accolite_Job_Portal.Models.SkillsModels.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class SkillsService : ISkillsService
    {
        private readonly ApplicationDbContext _dbContext;
        public SkillsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public SkillsResponseViewModel GetSkillsById(Guid Id)
        {
            SkillsResponseViewModel response = new SkillsResponseViewModel();

            var skills = _dbContext.Skills.Find(Id);
            if (skills == null)
            {
                response.Status = 404;
                response.Message = "Skills with this Id does not exist";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved skills with given Id.";
            response.Skills = new SkillsViewModel(skills);
            return response;
        }

        public SkillsResponseViewModel GetSkillsOfACandidate(Guid CandidateId)
        {
            SkillsResponseViewModel response = new SkillsResponseViewModel();

            var candidate = _dbContext.Candidates.Find(CandidateId);
            if (candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            var skills = _dbContext.Skills.Where(item => item.CandidateId == CandidateId).FirstOrDefault();
            if (skills == null)
            {
                response.Status = 404;
                response.Message = "Skills of this candidate does not exist.";
                return response;
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

        public async Task<SkillsResponseViewModel> UpdateSkills(UpdateSkillsViewModel updateSkills)
        {
            SkillsResponseViewModel response = new SkillsResponseViewModel();

            var skills = _dbContext.Skills.Find(updateSkills.SkillsId);
            if (skills == null)
            {
                response.Status = 404;
                response.Message = "Skills with this Id does not exist";
                return response;
            }

            skills.CandidateSkills = updateSkills.CandidateSkills;

            _dbContext.Skills.Update(skills);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully updated that candidate experience.";
            response.Skills = new SkillsViewModel(skills);
            return response;
        }

        public async Task<SkillsResponseViewModel> RemoveSkills(Guid Id)
        {
            SkillsResponseViewModel response = new SkillsResponseViewModel();

            var skills = _dbContext.Skills.Find(Id);
            if (skills == null)
            {
                response.Status = 404;
                response.Message = "Skills with this Id does not exist";
                return response;
            }

            _dbContext.Skills.Remove(skills);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully removed that resume.";
            response.Skills = new SkillsViewModel(skills);
            return response;
        }
    }
}
