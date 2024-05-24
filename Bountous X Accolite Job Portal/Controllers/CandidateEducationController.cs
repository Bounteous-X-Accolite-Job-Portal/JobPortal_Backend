using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel;
using Bountous_X_Accolite_Job_Portal.Models.CandidateEducationViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CandidateEducationController : ControllerBase
    {
        private readonly ICandidateEducationService _candidateEducationService;
        public CandidateEducationController(ICandidateEducationService candidateEducationService)
        {
            _candidateEducationService = candidateEducationService;
        }

        [HttpGet]
        [Route("getAllEducationDetails/{CandidateId}")]
        public async Task<MultipleEducationResponseViewModel> GetAllEducationOfACandidate(Guid CandidateId)
        {
            MultipleEducationResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee && candidateId != CandidateId)
            {
                response = new MultipleEducationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to get Education details.";
                return response;
            }

            response = await _candidateEducationService.GetAllEducationOfACandidate(CandidateId);
            return response;
        }

        [HttpGet]
        [Route("getDetails/{Id}")]
        public async Task<CandidateEducationResponseViewModel> GetEducationById(Guid Id)
        {
            CandidateEducationResponseViewModel response = await _candidateEducationService.GetEducationById((Guid)Id);
            if (response.CandidateEducation == null)
            {
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee && candidateId != response.CandidateEducation.CandidateId)
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (isEmployee)
            {
                response = new CandidateEducationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add Education.";
                return response;
            }

            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            response = await _candidateEducationService.AddCandidateEducation(addEducation, candidateId);
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (isEmployee)
            {
                response = new CandidateEducationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update Education.";
                return response;
            }

            CandidateEducationResponseViewModel education = await _candidateEducationService.GetEducationById((Guid)updateEducation.EducationId);
            if (education.CandidateEducation == null)
            {
                return education;
            }

            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (education.CandidateEducation.CandidateId == null || candidateId != education.CandidateEducation.CandidateId)
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            if (isEmployee)
            {
                response = new CandidateEducationResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove Education.";
                return response;
            }

            CandidateEducationResponseViewModel education = await _candidateEducationService.GetEducationById(Id);
            if(education.CandidateEducation == null)
            {
                return education;
            }

            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (education.CandidateEducation.CandidateId == null || candidateId != education.CandidateEducation.CandidateId)
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
