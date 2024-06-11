using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.SkillsModels;
using Bountous_X_Accolite_Job_Portal.Models.SkillsModels.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillsService _skillsService;
        public SkillsController(ISkillsService skillsService)
        {
            _skillsService = skillsService;
        }

        [HttpGet]
        [Route("getSkills/{CandidateId}")]
        public async Task<SkillsResponseViewModel> GetSkillsOfACandidate(Guid CandidateId)
        {
            SkillsResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee && candidateId != CandidateId)
            {
                response = new SkillsResponseViewModel();
                response.Status = 403;
                response.Message = "You are either not loggedIn or not authorized to get skills.";
                return response;
            }

            response = await _skillsService.GetSkillsOfACandidate(CandidateId);
            return response;
        }

        [HttpGet]
        [Route("skills/{Id}")]
        public async Task<SkillsResponseViewModel> GetSkillsById(Guid Id)
        {
            SkillsResponseViewModel response = await _skillsService.GetSkillsById(Id);
            if (response.Skills == null)
            {
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (response.Skills.CandidateId == null || (!isEmployee && candidateId != response.Skills.CandidateId))
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (isEmployee || candidateId == Guid.Empty)
            {
                response = new SkillsResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add skills.";
                return response;
            }

            response = await _skillsService.AddSkills(addSkills, candidateId);
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (isEmployee)
            {
                response = new SkillsResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to update skills.";
                return response;
            }

            response = await _skillsService.UpdateSkills(updateSkills, candidateId);
            return response;
        }
    }
}
