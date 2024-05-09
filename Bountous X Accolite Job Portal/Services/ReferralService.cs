using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels;
using Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel;
using Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class ReferralService : IReferralService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICandidateAccountService _candidateAccountService;
        private readonly IJobStatusService _jobStatusService;
        public ReferralService(ApplicationDbContext dbContext, ICandidateAccountService candidateAccountService, IJobStatusService jobStatusService)
        {
            _dbContext = dbContext;
            _candidateAccountService = candidateAccountService;
            _jobStatusService = jobStatusService;
        }

        public async Task<ReferralResponseViewModel> Refer(AddReferralViewModel addReferral, Guid EmpId)
        {
            ReferralResponseViewModel response;
            Guid candidateId = Guid.Empty;

            int referralStatusId = _jobStatusService.getInitialReferralStatus();
            if(referralStatusId == -1)
            {
                response = new ReferralResponseViewModel();
                response.Status = 404;
                response.Message = "Referral Status i.e 'ReferralStatusId' not found in database";
                return response;
            }

            var user = _dbContext.Users.Where(item => String.Equals(item.Email, addReferral.Email)).FirstOrDefault();
            if(user == null)
            {
                CandidateRegisterViewModel newCandidate = new CandidateRegisterViewModel();
                newCandidate.Email = addReferral.Email;
                newCandidate.FirstName = addReferral.FirstName;
                newCandidate.LastName = addReferral.LastName;
                newCandidate.Password = GeneratePassword.GenerateRandomPassword();

                CandidateResponseViewModel candidate = await _candidateAccountService.Register(newCandidate);
                if(candidate.Candidate == null)
                {
                    response = new ReferralResponseViewModel();
                    response.Status = candidate.Status;
                    response.Message = "Unable to create candidate, please try again.";
                    return response;
                }

                candidateId = candidate.Candidate.CandidateId;
            }
            else
            {
                candidateId = (Guid)user.CandidateId;
            }

            Referral referral = new Referral();
            referral.CandidateId = candidateId;
            referral.JobId = addReferral.JobId;
            referral.StatusId = referralStatusId;
            referral.EmpId = EmpId;
            
            await _dbContext.Referrals.AddAsync(referral);
            await _dbContext.SaveChangesAsync();

            response = new ReferralResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully referred!";
            response.Referral = new ReferralViewModel(referral);
            return response;
        }

        public AllReferralResponseViewModel GetAllReferralsOfLoggedInEmployee(Guid EmpId)
        {
            AllReferralResponseViewModel response = new AllReferralResponseViewModel();

            List<Referral> referrals = _dbContext.Referrals.Where(item => item.EmpId == EmpId).ToList();

            List<ReferralViewModel> allReferrals = new List<ReferralViewModel>();
            foreach(Referral referral in referrals)
            {
                allReferrals.Add(new ReferralViewModel(referral));
            }

            response.Status = 200;
            response.Message = "Successfully retrieved all referrals of loggedIn candidate";
            response.Referrals = allReferrals;
            return response;
        }
    }
}
