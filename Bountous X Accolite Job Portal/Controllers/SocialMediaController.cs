using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.SocialMediaViewModel;
using Bountous_X_Accolite_Job_Portal.Models.SocialMediaViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SocialMediaController : ControllerBase
    {
        private readonly ISocialMediaService _socialMediaService;
        public SocialMediaController(ISocialMediaService socialMediaService)
        {
            _socialMediaService = socialMediaService;
        }

        [HttpGet]
        [Route("getSocialMediaDetails/{CandidateId}")]
        public async Task<SocialMediaResponseViewModel> GetSocialMediaDetailsOfACandidate(Guid CandidateId)
        {
            SocialMediaResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("CandidateId"));
            if (!isEmployee && candidateId != CandidateId)
            {
                response = new SocialMediaResponseViewModel();
                response.Status = 403;
                response.Message = "You are either not loggedIn or not authorized to get social media details.";
                return response;
            }

            response = _socialMediaService.GetSocialMediaOfACandidate(CandidateId);
            return response;
        }

        [HttpGet]
        [Route("socialMediaDetails/{Id}")]
        public async Task<SocialMediaResponseViewModel> GetSocialMediaDetailsById(Guid Id)
        {
            SocialMediaResponseViewModel response = _socialMediaService.GetSocialMediaById(Id);
            if (response.SocialMedia == null)
            {
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("CandidateId"));
            if (response.SocialMedia.CandidateId == null || (!isEmployee && candidateId != response.SocialMedia.CandidateId))
            {
                SocialMediaResponseViewModel res = new SocialMediaResponseViewModel();
                res.Status = 401;
                res.Message = "You are either not loggedIn or not authorized to get social media details.";
                return res;
            }

            return response;
        }

        [HttpPost]
        [Route("addSocialMediaDetails")]
        public async Task<SocialMediaResponseViewModel> AddSocialMediaDetails(AddSocialMediaViewModel addSocialMedia)
        {
            SocialMediaResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new SocialMediaResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("CandidateId"));
            if (isEmployee || candidateId == Guid.Empty)
            {
                response = new SocialMediaResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add social media details.";
                return response;
            }

            response = await _socialMediaService.AddSocialMedia(addSocialMedia, candidateId);
            return response;
        }

        [HttpPut]
        [Route("updateSocialMediaDetails")]
        public async Task<SocialMediaResponseViewModel> UpdateSocialMediaDetails(UpdateSocialMediaViewModel updateSocialMedia)
        {
            SocialMediaResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new SocialMediaResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("CandidateId"));
            if (isEmployee)
            {
                response = new SocialMediaResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update social media details.";
                return response;
            }

            SocialMediaResponseViewModel socialMedia = _socialMediaService.GetSocialMediaById(updateSocialMedia.SocialMediaId);
            if (socialMedia.SocialMedia == null)
            {
                return socialMedia;
            }

            if (socialMedia.SocialMedia.CandidateId == null || candidateId != socialMedia.SocialMedia.CandidateId)
            {
                response = new SocialMediaResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update social media details.";
                return response;
            }

            response = await _socialMediaService.UpdateSocialMedia(updateSocialMedia);
            return response;
        }

        [HttpDelete]
        [Route("removeSocialMediaDetails/{Id}")]
        public async Task<SocialMediaResponseViewModel> RemoveSocialMediaDetails(Guid Id)
        {
            SocialMediaResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("CandidateId"));
            if (isEmployee)
            {
                response = new SocialMediaResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove social media details.";
                return response;
            }

            SocialMediaResponseViewModel socialMedia = _socialMediaService.GetSocialMediaById(Id);
            if (socialMedia.SocialMedia == null)
            {
                return socialMedia;
            }

            if (socialMedia.SocialMedia.CandidateId == null || candidateId != socialMedia.SocialMedia.CandidateId)
            {
                response = new SocialMediaResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove social media details.";
                return response;
            }

            response = await _socialMediaService.RemoveSocialMedia(Id);
            return response;
        }

    }
}
