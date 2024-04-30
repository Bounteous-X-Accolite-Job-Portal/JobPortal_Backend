using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models.SocialMediaViewModel;
using Bountous_X_Accolite_Job_Portal.Models.SocialMediaViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Bountous_X_Accolite_Job_Portal.Models;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly ApplicationDbContext _dbContext;
        public SocialMediaService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SocialMediaResponseViewModel GetSocialMediaOfACandidate(Guid CandidateId)
        {
            SocialMediaResponseViewModel response = new SocialMediaResponseViewModel();

            var candidate = _dbContext.Candidates.Find(CandidateId);
            if (candidate == null)
            {
                response.Status = 404;
                response.Message = "Candidate with this Id does not exist";
                return response;
            }

            var socialMedia = _dbContext.SocialMedia.Where(item => item.CandidateId == CandidateId).FirstOrDefault();
            if (socialMedia == null)
            {
                response.Status = 404;
                response.Message = "Social Media details of this candidate does not exist.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved the social media details of given candidate.";
            response.SocialMedia = new SocialMediaViewModel(socialMedia);
            return response;
        }

        public SocialMediaResponseViewModel GetSocialMediaById(Guid Id)
        {
            SocialMediaResponseViewModel response = new SocialMediaResponseViewModel();

            var socialMedia = _dbContext.SocialMedia.Find(Id);
            if (socialMedia == null)
            {
                response.Status = 404;
                response.Message = "Social Media with this Id does not exist";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved candidate social media details with given Id.";
            response.SocialMedia = new SocialMediaViewModel(socialMedia);
            return response;
        }

        public async Task<SocialMediaResponseViewModel> AddSocialMedia(AddSocialMediaViewModel addSocialMedia, Guid CandidateId)
        {
            SocialMedia socialMedia = new SocialMedia
            {
                CandidateId = CandidateId,
                Link1 = addSocialMedia.Link1,
                Link2 = addSocialMedia.Link2,
                Link3 = addSocialMedia.Link3
            };

            await _dbContext.SocialMedia.AddAsync(socialMedia);
            await _dbContext.SaveChangesAsync();

            SocialMediaResponseViewModel response = new SocialMediaResponseViewModel();

            if (socialMedia == null)
            {
                response.Status = 500;
                response.Message = "Unable to add social media details, please try again.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully added candidate social media details.";
            response.SocialMedia = new SocialMediaViewModel(socialMedia);
            return response;
        }

        public async Task<SocialMediaResponseViewModel> UpdateSocialMedia(UpdateSocialMediaViewModel updateSocialMedia)
        {
            SocialMediaResponseViewModel response = new SocialMediaResponseViewModel();

            var socialMedia = _dbContext.SocialMedia.Find(updateSocialMedia.SocialMediaId);
            if (socialMedia == null)
            {
                response.Status = 404;
                response.Message = "Social Media with this Id does not exist";
                return response;
            }

            socialMedia.Link1 = updateSocialMedia.Link1;
            socialMedia.Link2 = updateSocialMedia.Link2;
            socialMedia.Link3 = updateSocialMedia.Link3;

            _dbContext.SocialMedia.Update(socialMedia);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully updated that candidate social media details.";
            response.SocialMedia = new SocialMediaViewModel(socialMedia);
            return response;
        }

        public async Task<SocialMediaResponseViewModel> RemoveSocialMedia(Guid Id)
        {
            SocialMediaResponseViewModel response = new SocialMediaResponseViewModel();

            var socialMedia = _dbContext.SocialMedia.Find(Id);
            if (socialMedia == null)
            {
                response.Status = 404;
                response.Message = "Social Media with this Id does not exist";
                return response;
            }

            _dbContext.SocialMedia.Remove(socialMedia);
            await _dbContext.SaveChangesAsync();

            response.Status = 200;
            response.Message = "Successfully removed that social media details.";
            response.SocialMedia = new SocialMediaViewModel(socialMedia);
            return response;
        }
    }
}
