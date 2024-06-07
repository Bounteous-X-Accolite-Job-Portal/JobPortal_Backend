using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.ResumeModels;
using Bountous_X_Accolite_Job_Portal.Models.ResumeModels.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class ResumeService : IResumeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDistributedCache _cache;
        private readonly ICandidateAccountService _candidateAccountService;
        public ResumeService(ApplicationDbContext dbContext, IDistributedCache cache, ICandidateAccountService candidateAccountService)
        {
            _dbContext = dbContext;
            _cache = cache;
            _candidateAccountService = candidateAccountService;
        }

        public async Task<ResumeResponseViewModel> GetResumeById(Guid Id)
        {
            ResumeResponseViewModel response = new ResumeResponseViewModel();

            string key = $"getResumeById-{Id}";
            string? getResumeByIdFromCache = await _cache.GetStringAsync(key);

            Resume resume;
            if (string.IsNullOrEmpty(getResumeByIdFromCache))
            {
                resume = _dbContext.Resumes.Find(Id);
                if (resume == null)
                {
                    response.Status = 404;
                    response.Message = "Resume with this Id does not exist";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(resume));
            }
            else
            {
                resume = JsonSerializer.Deserialize<Resume>(getResumeByIdFromCache);
            }

            response.Status = 200;
            response.Message = "Successfully retrieved candidate resume with given Id.";
            response.Resume = new ResumeViewModel(resume);
            return response;
        }

        public async Task<ResumeResponseViewModel> GetResumeOfACandidate(Guid CandidateId)
        {
            ResumeResponseViewModel response = new ResumeResponseViewModel();

            var candidate = await _candidateAccountService.GetCandidateById(CandidateId);
            if (candidate.Candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            string key = $"getResumeByCandidateId-{CandidateId}";
            string? getResumeByCandidateIdFromCache = await _cache.GetStringAsync(key);

            Resume resume;
            if (string.IsNullOrEmpty(getResumeByCandidateIdFromCache))
            {
                resume = _dbContext.Resumes.Where(item => item.CandidateId == CandidateId).FirstOrDefault();
                if (resume == null)
                {
                    response.Status = 404;
                    response.Message = "Resume of this candidate does not exist.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(resume));
            }
            else
            {
                resume = JsonSerializer.Deserialize<Resume>(getResumeByCandidateIdFromCache);
            }

            response.Status = 200;
            response.Message = "Successfully retrieved the resume of given candidate.";
            response.Resume = new ResumeViewModel(resume);
            return response;
        }

        public async Task<ResumeResponseViewModel> AddResume(AddResumeViewModel addResume, Guid CandidateId)
        {
            Resume resume = new Resume
            {
                ResumeUrl = addResume.ResumeUrl,
                CandidateId = CandidateId
            };

            await _dbContext.Resumes.AddAsync(resume);
            await _dbContext.SaveChangesAsync();

            ResumeResponseViewModel response = new ResumeResponseViewModel();

            if (resume == null)
            {
                response.Status = 500;
                response.Message = "Unable to add resume, please try again.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully added candidate resume.";
            response.Resume = new ResumeViewModel(resume);
            return response;
        }

        public async Task<ResumeResponseViewModel> RemoveResume(Guid Id, Guid CandidateId)
        {
            ResumeResponseViewModel response = new ResumeResponseViewModel();

            string key = $"getResumeById-{Id}";
            string? getResumeByIdFromCache = await _cache.GetStringAsync(key);

            Resume resume;
            if (string.IsNullOrEmpty(getResumeByIdFromCache))
            {
                resume = _dbContext.Resumes.Find(Id);
                if (resume == null)
                {
                    response.Status = 404;
                    response.Message = "Resume with this Id does not exist";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(resume));
            }
            else
            {
                resume = JsonSerializer.Deserialize<Resume>(getResumeByIdFromCache);
            }

            if(resume.CandidateId != CandidateId)
            {
                response.Status = 401;
                response.Message = "You are either not loggedIn or not authorized to remove resume.";
                return response;
            }

            _dbContext.Resumes.Remove(resume);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"getResumeById-{Id}");
            await _cache.RemoveAsync($"getResumeByCandidateId-{resume.CandidateId}");

            response.Status = 200;
            response.Message = "Successfully removed that resume.";
            response.Resume = new ResumeViewModel(resume);
            return response;
        }
    }
}
