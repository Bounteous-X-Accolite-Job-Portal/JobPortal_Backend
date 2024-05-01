using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.SkillsViewModel;
using Bountous_X_Accolite_Job_Portal.Models.SkillsViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SkillsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ISkillsService _skillsService;
        public SkillsController(UserManager<User> userManager, ISkillsService skillsService)
        {
            _skillsService = skillsService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("getSkills/{CandidateId}")]
        public async Task<SkillsResponseViewModel> GetSkillsOfACandidate(Guid CandidateId)
        {
            SkillsResponseViewModel response;

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (user.CandidateId != null && user.CandidateId != CandidateId))
            {
                response = new SkillsResponseViewModel();
                response.Status = 403;
                response.Message = "You are either not loggedIn or not authorized to get skills.";
                return response;
            }

            response = _skillsService.GetSkillsOfACandidate(CandidateId);
            return response;
        }

        [HttpGet]
        [Route("skills/{Id}")]
        public async Task<SkillsResponseViewModel> GetSkillsById(Guid Id)
        {
            SkillsResponseViewModel response = _skillsService.GetSkillsById(Id);
            if (response.Skills == null)
            {
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (user.CandidateId != null && response.Skills.CandidateId != null && user.CandidateId != response.Skills.CandidateId))
            {
                SkillsResponseViewModel res = new SkillsResponseViewModel();
                res.Status = 401;
                res.Message = "You are either not loggedIn or not authorized to get skills.";
                return res;
            }

            return response;
        }

        [HttpPost]
        [Route("addSkills")]
        public async Task<SkillsResponseViewModel> AddSkills(AddSkillsViewModel addSkills)
        {
            SkillsResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new SkillsResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
            {
                response = new SkillsResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add skills.";
                return response;
            }

            response = await _skillsService.AddSkills(addSkills, (Guid)user.CandidateId);
            return response;
        }

        [HttpPut]
        [Route("updateSkills")]
        public async Task<SkillsResponseViewModel> UpdateSkills(UpdateSkillsViewModel updateSkills)
        {
            SkillsResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new SkillsResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
            {
                response = new SkillsResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update skills.";
                return response;
            }

            SkillsResponseViewModel skills = _skillsService.GetSkillsById(updateSkills.SkillsId);
            if (skills.Skills == null)
            {
                return skills;
            }

            if (skills.Skills.CandidateId == null || user.CandidateId != skills.Skills.CandidateId)
            {
                response = new SkillsResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update skills.";
                return response;
            }

            response = await _skillsService.UpdateSkills(updateSkills);
            return response;
        }

        [HttpDelete]
        [Route("removeSkills/{Id}")]
        public async Task<SkillsResponseViewModel> RemoveSkills(Guid Id)
        {
            SkillsResponseViewModel response;

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
            {
                response = new SkillsResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove skills.";
                return response;
            }

            SkillsResponseViewModel skills = _skillsService.GetSkillsById(Id);
            if (skills.Skills == null)
            {
                return skills;
            }

            if (skills.Skills.CandidateId == null || user.CandidateId != skills.Skills.CandidateId)
            {
                response = new SkillsResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove skills.";
                return response;
            }

            response = await _skillsService.RemoveSkills(Id);
            return response;
        }
    }
}
