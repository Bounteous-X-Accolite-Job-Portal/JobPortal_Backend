using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CandidateExperienceController : ControllerBase
    {
        private readonly ICandidateExperienceService _candidateExperienceService;
        private readonly UserManager<User> _userManager;
        public CandidateExperienceController(ICandidateExperienceService candidateExperienceService, UserManager<User> userManager)
        {
            _candidateExperienceService = candidateExperienceService;
            _userManager = userManager; 
        }

        [HttpGet]
        [Route("getAllExperienceDetails/{CandidateId}")]
        public async Task<MultipleExperienceResponseViewModel> GetAllExperienceOfACandidate(Guid CandidateId)
        {
            MultipleExperienceResponseViewModel response;

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (user.CandidateId != null && user.CandidateId != CandidateId))
            {
                response = new MultipleExperienceResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to get Experience details.";
                return response;
            }

            response = _candidateExperienceService.GetAllExperienceOfACandidate(CandidateId);
            return response;
        }

        [HttpGet]
        [Route("getExperience/{Id}")]
        public async Task<CandidateExperienceResponseViewModel> GetExperienceById(Guid Id)
        {
            CandidateExperienceResponseViewModel response = _candidateExperienceService.GetExperienceById(Id);
            if (response.Experience == null)
            {
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            // checking logged in Candidate and created by Candidate are same person
            if (user == null || (user.CandidateId != null && response.Experience.CandidateId != null && user.CandidateId != response.Experience.CandidateId))
            {
                CandidateExperienceResponseViewModel res = new CandidateExperienceResponseViewModel();
                res.Status = 401;
                res.Message = "You are either not loggedIn or not authorized to get Experience details.";
                return res;
            }

            return response;
        }

        [HttpPost]
        [Route("addExperience")]
        public async Task<CandidateExperienceResponseViewModel> AddExperience(AddCandidateExperienceViewModel addExperience)
        {
            CandidateExperienceResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new CandidateExperienceResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
            {
                response = new CandidateExperienceResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add Experience.";
                return response;
            }

            response = await _candidateExperienceService.AddCandidateExperience(addExperience, (Guid)user.CandidateId);
            return response;
        }

        [HttpPut]
        [Route("updateExperience")]
        public async Task<CandidateExperienceResponseViewModel> UpdateExperience(UpdateCandidateExperienceViewModel updateExperience)
        {
            CandidateExperienceResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new CandidateExperienceResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
            {
                response = new CandidateExperienceResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update Experience.";
                return response;
            }

            CandidateExperienceResponseViewModel experience = _candidateExperienceService.GetExperienceById(updateExperience.ExperienceId);
            if (experience.Experience == null)
            {
                return experience;
            }

            if (experience.Experience.CandidateId == null || user.CandidateId != experience.Experience.CandidateId)
            {
                response = new CandidateExperienceResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update Experience.";
                return response;
            }

            response = await _candidateExperienceService.UpdateCandidateExperience(updateExperience);
            return response;
        }

        [HttpDelete]
        [Route("removeExperience/{Id}")]
        public async Task<CandidateExperienceResponseViewModel> RemoveExperience(Guid Id)
        {
            CandidateExperienceResponseViewModel response;

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
            {
                response = new CandidateExperienceResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove Experience.";
                return response;
            }

            CandidateExperienceResponseViewModel experience = _candidateExperienceService.GetExperienceById(Id);
            if (experience.Experience == null)
            {
                return experience;
            }

            if (experience.Experience.CandidateId == null || user.CandidateId != experience.Experience.CandidateId)
            {
                response = new CandidateExperienceResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove Experience.";
                return response;
            }

            response = await _candidateExperienceService.RemoveExperience(Id);
            return response;
        }
    }
}


