using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models.ResumeModels;
using Bountous_X_Accolite_Job_Portal.Models.ResumeModels.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;
        public ResumeController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        [HttpGet]
        [Route("getResume/{CandidateId}")]
        public async Task<ResumeResponseViewModel> GetResumeOfACandidate(Guid CandidateId)
        {
            ResumeResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (!isEmployee && candidateId != CandidateId)
            {
                response = new ResumeResponseViewModel();
                response.Status = 403;
                response.Message = "You are either not loggedIn or not authorized to get Resume.";
                return response;
            }

            response = await _resumeService.GetResumeOfACandidate(CandidateId);
            return response;
        }

        [HttpGet]
        [Route("resume/{Id}")]
        public async Task<ResumeResponseViewModel> GetResumeById(Guid Id)
        {
            ResumeResponseViewModel response = await _resumeService.GetResumeById(Id);
            if (response.Resume == null)
            {
                return response;
            }

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (response.Resume.CandidateId == null || (!isEmployee && candidateId != response.Resume.CandidateId))
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

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (isEmployee || candidateId == Guid.Empty)
            {
                response = new ResumeResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to add resume.";
                return response;
            }

            response = await _resumeService.AddResume(addResume, candidateId);
            return response;
        }

        [HttpDelete]
        [Route("removeResume/{Id}")]
        public async Task<ResumeResponseViewModel> RemoveResume(Guid Id)
        {
            ResumeResponseViewModel response;

            bool isEmployee = Convert.ToBoolean(User.FindFirstValue("IsEmployee"));
            Guid candidateId = GetGuidFromString.Get(User.FindFirstValue("Id"));
            if (isEmployee)
            {
                response = new ResumeResponseViewModel();
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove resume.";
                return response;
            }

            response = await _resumeService.RemoveResume(Id, candidateId);
            return response;
        }
    }
}
