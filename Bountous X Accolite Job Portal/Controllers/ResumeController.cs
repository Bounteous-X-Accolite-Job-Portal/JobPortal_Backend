using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.ResumeViewModel;
using Bountous_X_Accolite_Job_Portal.Models.ResumeViewModel.ResponseViewModels;
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
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;
        private readonly UserManager<User> _userManager;
        public ResumeController(IResumeService resumeService, UserManager<User> userManager)
        {
            _resumeService = resumeService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("getResume/{CandidateId}")]
        public async Task<ResumeResponseViewModel> GetResumeOfACandidate(Guid CandidateId)
        {
            ResumeResponseViewModel response;

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (user.CandidateId != null && user.CandidateId != CandidateId))
            {
                response = new ResumeResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to get Resume.";
                return response;
            }

            response = _resumeService.GetResumeOfACandidate(CandidateId);
            return response;
        }

        [HttpGet]
        [Route("resume/{Id}")]
        public async Task<ResumeResponseViewModel> GetResumeById(Guid Id)
        {
            ResumeResponseViewModel response = _resumeService.GetResumeById(Id);
            if (response.Resume == null)
            {
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (user.CandidateId != null && response.Resume.CandidateId != null && user.CandidateId != response.Resume.CandidateId))
            {
                ResumeResponseViewModel res = new ResumeResponseViewModel();
                res.Status = 401;
                res.Message = "You are either not loggedIn or not authorized to get resume.";
                return res;
            }

            return response;
        }

        [HttpPost]
        [Route("addResume")]
        public async Task<ResumeResponseViewModel> AddResume(AddResumeViewModel addResume)
        {
            ResumeResponseViewModel response;

            if (!ModelState.IsValid)
            {
                response = new ResumeResponseViewModel();
                response.Status = 422;
                response.Message = "Please Enter all the details.";
                return response;
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
            {
                response = new ResumeResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add resume.";
                return response;
            }

            response = await _resumeService.AddResume(addResume, (Guid)user.CandidateId);
            return response;
        }

        [HttpDelete]
        [Route("removeResume/{Id}")]
        public async Task<ResumeResponseViewModel> RemoveResume(Guid Id)
        {
            ResumeResponseViewModel response;

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.EmpId != null)
            {
                response = new ResumeResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove resume.";
                return response;
            }

            ResumeResponseViewModel education = _resumeService.GetResumeById(Id);
            if (education.Resume == null)
            {
                return education;
            }

            if (education.Resume.CandidateId == null || user.CandidateId != education.Resume.CandidateId)
            {
                response = new ResumeResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove resume.";
                return response;
            }

            response = await _resumeService.RemoveResume(Id);
            return response;
        }
    }
}
