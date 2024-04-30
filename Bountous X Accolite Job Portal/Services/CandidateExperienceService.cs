using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel;
using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class CandidateExperienceService : ICandidateExperienceService
    {
        private readonly ApplicationDbContext _dbContext;
        public CandidateExperienceService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public MultipleExperienceResponseViewModel GetAllExperienceOfACandidate(Guid CandidateId)
        {
            MultipleExperienceResponseViewModel response = new MultipleExperienceResponseViewModel();

            var candidate = _dbContext.Candidates.Find(CandidateId);
            if (candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            List<CandidateExperience> allExperienceOfCandidate = _dbContext.CandidateExperience.Where(item => item.CandidateId == CandidateId).ToList();

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

        public CandidateExperienceResponseViewModel GetExperienceById(Guid Id)
        {
            CandidateExperienceResponseViewModel response = new CandidateExperienceResponseViewModel();

            var experience = _dbContext.CandidateExperience.Find(Id);
            if (experience == null)
            {
                response.Status = 404;
                response.Message = "Candidate Experience with this Id does not exist";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved candidate experience with given Id.";
            response.Experience = new CandidateExperienceViewModel(experience);
            return response;
        }

        public async Task<CandidateExperienceResponseViewModel> AddCandidateExperience(AddCandidateExperienceViewModel addExperience, Guid CandidateId)
        {
            CandidateExperienceResponseViewModel response = new CandidateExperienceResponseViewModel();

            var company = _dbContext.Company.Find(addExperience.CompanyId);
            if (company == null)
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

            response.Status = 200;
            response.Message = "Successfully added candidate experience.";
            response.Experience = new CandidateExperienceViewModel(experienceToBeAdded);
            return response;
        }

        public async Task<CandidateExperienceResponseViewModel> UpdateCandidateExperience(UpdateCandidateExperienceViewModel updateExperience)
        {
            CandidateExperienceResponseViewModel response = new CandidateExperienceResponseViewModel();

            var experience = _dbContext.CandidateExperience.Find(updateExperience.ExperienceId);
            if (experience == null)
            {
                response.Status = 404;
                response.Message = "The candidate experience you are trying to update does not exist in database.";
                return response;
            }

            var company = _dbContext.Company.Find(updateExperience.CompanyId);
            if (company == null)
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

            response.Status = 200;
            response.Message = "Successfully updated that candidate experience.";
            response.Experience = new CandidateExperienceViewModel(experience);
            return response;
        }

        public async Task<CandidateExperienceResponseViewModel> RemoveExperience(Guid Id)
        {
            CandidateExperienceResponseViewModel response = new CandidateExperienceResponseViewModel();

            var experience = _dbContext.CandidateExperience.Find(Id);
            if (experience == null)
            {
                response.Status = 404;
                response.Message = "The candidate experience you are trying to remove does not exist in database.";
                return response;
            }

            _dbContext.CandidateExperience.Remove(experience);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully removed that experience.";
            response.Experience = new CandidateExperienceViewModel(experience);
            return response;
        }
    }
}
