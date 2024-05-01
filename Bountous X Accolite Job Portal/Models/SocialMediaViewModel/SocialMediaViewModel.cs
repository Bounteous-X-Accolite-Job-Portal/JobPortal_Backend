namespace Bountous_X_Accolite_Job_Portal.Models.SocialMediaViewModel
{
    public class SocialMediaViewModel
    {
        public Guid SocialMediaId { get; set; }
        public string Link1 { get; set; }
        public string Link2 { get; set; }
        public string Link3 { get; set; }
        public Guid? CandidateId { get; set; }

        public SocialMediaViewModel(SocialMedia socialMedia)
        {
            SocialMediaId = socialMedia.SocialMediaId;
            Link1 = socialMedia.Link1;
            Link2 = socialMedia.Link2;
            Link3 = socialMedia.Link3;
            CandidateId = socialMedia.CandidateId;
        }
    }
}
