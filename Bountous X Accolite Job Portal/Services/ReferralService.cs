﻿using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels;
using Bountous_X_Accolite_Job_Portal.Models.EMAIL;
using Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel;
using Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using Bountous_X_Accolite_Job_Portal.Models.JobApplicationModels;
using Microsoft.AspNetCore.Builder;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class ReferralService : IReferralService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICandidateAccountService _candidateAccountService;
        private readonly IJobStatusService _jobStatusService;
        private readonly IDistributedCache _cache;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        public ReferralService(ApplicationDbContext dbContext, ICandidateAccountService candidateAccountService, IJobStatusService jobStatusService,UserManager<User> userManager, IEmailService emailService, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _candidateAccountService = candidateAccountService;
            _jobStatusService = jobStatusService;
            _userManager = userManager;
            _emailService = emailService;
            _cache = cache;
        }

        public async Task<ReferralResponseViewModel> Refer(AddReferralViewModel addReferral, Guid EmpId)
        {
            ReferralResponseViewModel response;
            Guid candidateId = Guid.Empty;

            int referralStatusId = await _jobStatusService.getInitialReferralStatus();
            if (referralStatusId == -1)
            {
                response = new ReferralResponseViewModel();
                response.Status = 404;
                response.Message = "Referral Status i.e 'ReferralStatusId' not found in database";
                return response;
            }

            var user = _dbContext.Users.Where(item => String.Equals(item.Email, addReferral.Email)).FirstOrDefault();
            if(user != null && user.EmpId != null)
            {
                response = new ReferralResponseViewModel();
                response.Status = 403;
                response.Message = "You are refering an employee which is not authorised.";
                return response;
            }

            // check and change referal status
            string key = $"getAllReferrals";
            string? getAllReferralsFromCache = await _cache.GetStringAsync(key);

            List<Referral> referrals;
            if (string.IsNullOrWhiteSpace(getAllReferralsFromCache))
            {
                referrals = _dbContext.Referrals.ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(referrals));
            }
            else
            {
                referrals = JsonSerializer.Deserialize<List<Referral>>(getAllReferralsFromCache);
            }

            foreach (var item in referrals)
            {
                if (user != null && item.JobId == addReferral.JobId && item.CandidateId == user.CandidateId)
                {
                    response = new ReferralResponseViewModel();
                    response.Status = 409;
                    response.Message = "This user has already been referred.";
                    return response;
                }
            }

            if (user == null)
            {
                CandidateRegisterViewModel newCandidate = new CandidateRegisterViewModel();
                newCandidate.Email = addReferral.Email;
                newCandidate.FirstName = addReferral.FirstName;
                newCandidate.LastName = addReferral.LastName;
                newCandidate.Password = GeneratePassword.GenerateRandomPassword();

                CandidateResponseViewModel candidate = await _candidateAccountService.Register(newCandidate);

                if (candidate.Candidate == null)
                {
                    response = new ReferralResponseViewModel();
                    response.Status = candidate.Status;
                    response.Message = "Unable to create candidate, please try again.";
                    return response;
                }

                var cand = await _userManager.FindByEmailAsync(newCandidate.Email);
                var tokenBytes = RandomNumberGenerator.GetBytes(64);
                var referalToken = Convert.ToBase64String(tokenBytes);
                cand.ReferalToken = referalToken;
                cand.AutoPassword = newCandidate.Password;

                EmailData emailRef = new EmailData(cand.Email, "bounteous x Accolite Job Portal!", ReferalEmailBody.EmailStringBody(newCandidate.FirstName, cand.Email, cand.ReferalToken, cand.AutoPassword));
                _emailService.SendEmail(emailRef);

                //await _dbContext.Users.AddAsync(cand);
                //await _dbContext.SaveChangesAsync();

                candidateId = candidate.Candidate.CandidateId;
            }
            else
            {
                var candRegisterd = await _userManager.FindByEmailAsync(user.Email);
                var token = RandomNumberGenerator.GetBytes(64);
                var referal = Convert.ToBase64String(token);
                candRegisterd.ReferalToken = referal;
                EmailData email = new EmailData(candRegisterd.Email, "You are being referred", ReferalBodyForAlreadyRegisterd.EmailStringBody(addReferral.FirstName));
                _emailService.SendEmail(email);

                candidateId = (Guid)user.CandidateId;
            }

            Referral referral = new Referral();
            referral.CandidateId = candidateId;
            referral.JobId = addReferral.JobId;
            referral.StatusId = referralStatusId;
            referral.EmpId = EmpId;
            
            await _dbContext.Referrals.AddAsync(referral);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"getAllReferralsByEmployeeId-{EmpId}");
            await _cache.RemoveAsync($"getAllReferrals");

            response = new ReferralResponseViewModel();
            response.Status = 200;
            response.Message = "Successfully referred!";
            response.Referral = new ReferralViewModel(referral);
            return response;    
        }

        public async Task<AllReferralResponseViewModel> GetAllReferralsOfLoggedInEmployee(Guid EmpId)
        {
            AllReferralResponseViewModel response = new AllReferralResponseViewModel();

            string key = $"getAllReferralsByEmployeeId-{EmpId}";
            string? getAllReferralsByEmployeeIdFromCache = await _cache.GetStringAsync(key);

            List<Referral> referrals;
            if (string.IsNullOrWhiteSpace(getAllReferralsByEmployeeIdFromCache))
            {
                referrals = _dbContext.Referrals.Where(item => item.EmpId == EmpId).ToList();
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(referrals));
            }
            else
            {
                referrals = JsonSerializer.Deserialize<List<Referral>>(getAllReferralsByEmployeeIdFromCache);
            }

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

        public async Task<bool> AddApplicationIdToReferral(Guid ReferralId, Guid ApplicationId)
        {
            var referral = _dbContext.Referrals.Find(ReferralId);
            if (referral == null)
            {
                return false;
            }

            referral.ApplicationId = ApplicationId;

            _dbContext.Referrals.Update(referral);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"getAllReferralsByEmployeeId-{referral.EmpId}");
            await _cache.RemoveAsync($"getAllReferrals");

            return true;
        }

        public async Task<bool> AddClosedApplicationIdToReferral(Guid ReferralId, Guid ClosedApplicationId)
        {
            var referral = _dbContext.Referrals.Find(ReferralId);
            if (referral == null)
            {
                return false;
            }

            referral.ApplicationId = null;
            referral.ClosedApplicationId = ClosedApplicationId;

            _dbContext.Referrals.Update(referral);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"getAllReferralsByEmployeeId-{referral.EmpId}");
            await _cache.RemoveAsync($"getAllReferrals");

            return true;
        }

        public async Task<bool> AddClosedJobIdToReferral(Referral referral, Guid ClosedJobId)
        {
            referral.JobId = null;
            referral.ClosedJobId = ClosedJobId;

            _dbContext.Referrals.Update(referral);

            await _cache.RemoveAsync($"getAllReferralsByEmployeeId-{referral.EmpId}");
            await _cache.RemoveAsync($"getAllReferrals");

            return true;
        }
    }
}
