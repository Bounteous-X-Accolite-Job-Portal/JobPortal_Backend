using Bountous_X_Accolite_Job_Portal.Models.SocialMediaViewModel;
using Bountous_X_Accolite_Job_Portal.Models.SocialMediaViewModel.ResponseViewModels;

namespace Bountous_X_Accolite_Job_Portal.Services.Abstract
{
    public interface ISocialMediaService
    {
        SocialMediaResponseViewModel GetSocialMediaOfACandidate(Guid CandidateId);
        SocialMediaResponseViewModel GetSocialMediaById(Guid Id);
        Task<SocialMediaResponseViewModel> AddSocialMedia(AddSocialMediaViewModel addSocialMedia, Guid CandidateId);
        Task<SocialMediaResponseViewModel> UpdateSocialMedia(UpdateSocialMediaViewModel updateSocialMedia);
        Task<SocialMediaResponseViewModel> RemoveSocialMedia(Guid Id);
    }
}
