using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CandidateEducationController : ControllerBase
    {
        private readonly ICandidateEducationService _candidateEducationService;
        private readonly UserManager<User> _userManager;
        public CandidateEducationController(ICandidateEducationService candidateEducationService, UserManager<User> userManager)
        {
            _candidateEducationService = candidateEducationService;
            _userManager = userManager; 
        }

        [HttpGet]
        [Route("getAllEducationDetails/{CandidateId}")]
        public async Task<MultipleEducationResponseViewModel> GetAllEducationOfACandidate(Guid CandidateId)
        {
            MultipleEducationResponseViewModel response;

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (user.CandidateId != null && user.CandidateId != CandidateId))
            {
                response = new MultipleEducationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to get Education details.";
                return response;
            }

            response = _candidateEducationService.GetAllEducationOfACandidate(CandidateId);
            return response;
        }

        [HttpGet]
        [Route("getDetails/{Id}")]
        public async Task<CandidateEducationResponseViewModel> GetEducationById(Guid Id)
        {
            CandidateEducationResponseViewModel response = _candidateEducationService.GetEducationById(Id);
            if (response.CandidateEducation == null)
            {
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            // checking logged in Candidate and created by Candidate are same person
            if (user == null || (user.CandidateId != null && response.CandidateEducation.CandidateId != null && user.CandidateId != response.CandidateEducation.CandidateId))
            {
                CandidateEducationResponseViewModel res = new CandidateEducationResponseViewModel();
                res.Status = 401;
                res.Message = "You are either not loggedIn or not authorized to get Education details.";
                return res;
            }

            return response;
        }

        [HttpPost]
        [Route("addEducation")]
        public async Task<CandidateEducationResponseViewModel> AddEducation(AddCandidateEducationViewModel addEducation)
        {
            CandidateEducationResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new CandidateEducationResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
            {
                response = new CandidateEducationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add Education.";
                return response;
            }

            response = await _candidateEducationService.AddCandidateEducation(addEducation, (Guid)user.CandidateId);
            return response;
        }

        [HttpPut]
        [Route("updateEducation")]
        public async Task<CandidateEducationResponseViewModel> UpdateEducation(UpdateCandidateEducationViewModel updateEducation)
        {
            CandidateEducationResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new CandidateEducationResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
            {
                response = new CandidateEducationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update Education.";
                return response;
            }

            CandidateEducationResponseViewModel education = _candidateEducationService.GetEducationById(updateEducation.EducationId);
            if (education.CandidateEducation == null)
            {
                return education;
            }

            if (education.CandidateEducation.CandidateId == null || user.CandidateId != education.CandidateEducation.CandidateId)
            {
                response = new CandidateEducationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update Education.";
                return response;
            }

            response = await _candidateEducationService.UpdateCandidateEducation(updateEducation);
            return response;
        }

        [HttpDelete]
        [Route("removeEducation/{Id}")]
        public async Task<CandidateEducationResponseViewModel> RemoveEducation(Guid Id)
        {
            CandidateEducationResponseViewModel response;

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
            {
                response = new CandidateEducationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove Education.";
                return response;
            }

            CandidateEducationResponseViewModel education = _candidateEducationService.GetEducationById(Id);
            if(education.CandidateEducation == null)
            {
                return education;
            }

            if (education.CandidateEducation.CandidateId == null || user.CandidateId != education.CandidateEducation.CandidateId)
            {
                response = new CandidateEducationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove Education.";
                return response;
            }

            response = await _candidateEducationService.RemoveEducation(Id);
            return response;
        }

    }
}
