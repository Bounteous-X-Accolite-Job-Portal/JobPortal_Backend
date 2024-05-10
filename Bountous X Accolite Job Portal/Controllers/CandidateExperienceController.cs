using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel;
using Bountous_X_Accolite_Job_Portal.Helpers;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CandidateExperienceController : ControllerBase
    {
        private readonly ICandidateExperienceService _candidateExperienceService;
        public CandidateExperienceController(ICandidateExperienceService candidateExperienceService)
        {
            _candidateExperienceService = candidateExperienceService;
        }

        [HttpGet]
        [Route("getAllExperienceDetails/{CandidateId}")]
        public async Task<MultipleExperienceResponseViewModel> GetAllExperienceOfACandidate(Guid CandidateId)
        {
            MultipleExperienceResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee && candidateId != CandidateId)
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee && candidateId != response.Experience.CandidateId)
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (isEmployee)
            {
                response = new CandidateExperienceResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add Experience.";
                return response;
            }

            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            response = await _candidateExperienceService.AddCandidateExperience(addExperience, candidateId);
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (isEmployee)
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

            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (experience.Experience.CandidateId == null || candidateId != experience.Experience.CandidateId)
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (isEmployee)
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

            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (experience.Experience.CandidateId == null || candidateId != experience.Experience.CandidateId)
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


