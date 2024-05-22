using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.ResumeModels;
using Bountous_X_Accolite_Job_Portal.Models.ResumeModels.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class ResumeService : IResumeService
    {
        private readonly ApplicationDbContext _dbContext;
        public ResumeService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResumeResponseViewModel GetResumeById(Guid Id)
        {
            ResumeResponseViewModel response = new ResumeResponseViewModel();

            var resume = _dbContext.Resumes.Find(Id);
            if (resume == null)
            {
                response.Status = 404;
                response.Message = "Resume with this Id does not exist";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved candidate resume with given Id.";
            response.Resume = new ResumeViewModel(resume);
            return response;
        }

        public ResumeResponseViewModel GetResumeOfACandidate(Guid CandidateId)
        {
            ResumeResponseViewModel response = new ResumeResponseViewModel();

            var candidate = _dbContext.Candidates.Find(CandidateId);
            if (candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            var resume = _dbContext.Resumes.Where(item => item.CandidateId == CandidateId).FirstOrDefault();
            if(resume == null)
            {
                response.Status = 404;
                response.Message = "Resume of this candidate does not exist.";
                return response;
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

        public async Task<ResumeResponseViewModel> RemoveResume(Guid Id)
        {
            ResumeResponseViewModel response = new ResumeResponseViewModel();

            var resume = _dbContext.Resumes.Find(Id);
            if (resume == null)
            {
                response.Status = 404;
                response.Message = "Resume with this Id does not exist";
                return response;
            }

            _dbContext.Resumes.Remove(resume);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully removed that resume.";
            response.Resume = new ResumeViewModel(resume);
            return response;
        }
    }
}
