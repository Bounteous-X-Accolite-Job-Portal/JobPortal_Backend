using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.CandidateExperienceViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Models.ResumeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.ResumeViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Models.SocialMediaViewModel;
using Bountous_X_Accolite_Job_Portal.Models.SocialMediaViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialMediaController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ISocialMediaService _socialMediaService;
        public SocialMediaController(ISocialMediaService socialMediaService, UserManager<User> userManager)
        {
            _socialMediaService = socialMediaService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("getSocialMediaDetails/{CandidateId}")]
        public async Task<SocialMediaResponseViewModel> GetSocialMediaDetailsOfACandidate(Guid CandidateId)
        {
            SocialMediaResponseViewModel response;

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (user.CandidateId != null && user.CandidateId != CandidateId))
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

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (user.CandidateId != null && response.SocialMedia.CandidateId != null && user.CandidateId != response.SocialMedia.CandidateId))
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

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
            {
                response = new SocialMediaResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add social media details.";
                return response;
            }

            response = await _socialMediaService.AddSocialMedia(addSocialMedia, (Guid)user.CandidateId);
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

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
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

            if (socialMedia.SocialMedia.CandidateId == null || user.CandidateId != socialMedia.SocialMedia.CandidateId)
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

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
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

            if (socialMedia.SocialMedia.CandidateId == null || user.CandidateId != socialMedia.SocialMedia.CandidateId)
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
