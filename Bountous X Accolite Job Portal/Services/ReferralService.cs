﻿using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Helpers;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels;
using Bountous_X_Accolite_Job_Portal.Models.EMAIL;
using Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel;
using Bountous_X_Accolite_Job_Portal.Models.ReferralViewModel.ResponseViewModels;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class ReferralService : IReferralService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICandidateAccountService _candidateAccountService;
        private readonly IJobStatusService _jobStatusService;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        public ReferralService(ApplicationDbContext dbContext, ICandidateAccountService candidateAccountService, IJobStatusService jobStatusService,UserManager<User> userManager, IEmailService emailService)
        {
            _dbContext = dbContext;
            _candidateAccountService = candidateAccountService;
            _jobStatusService = jobStatusService;
            _userManager = userManager;
            _emailService = emailService;
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
            if (user != null)
            {
                var candRegisterd = await _userManager.FindByEmailAsync(user.Email);
                var token = RandomNumberGenerator.GetBytes(64);
                var referal = Convert.ToBase64String(token);
                candRegisterd.ReferalToken = referal;
                EmailData email = new EmailData(candRegisterd.Email, "You are being referred", ReferalBodyForAlreadyRegisterd.EmailStringBody());
                _emailService.SendEmail(email);
            }




            if (user == null)
            {
                CandidateRegisterViewModel newCandidate = new CandidateRegisterViewModel();
                ConfirmPasswordDTO confirm=new ConfirmPasswordDTO();
                newCandidate.Email = addReferral.Email;
                newCandidate.FirstName = addReferral.FirstName;
                newCandidate.LastName = addReferral.LastName;
                newCandidate.Password = GeneratePassword.GenerateRandomPassword();
                var password=newCandidate.Password;



                CandidateResponseViewModel candidate = await _candidateAccountService.Register(newCandidate);
                var cand = await _userManager.FindByEmailAsync(newCandidate.Email);
                var tokenBytes = RandomNumberGenerator.GetBytes(64);
                var referalToken = Convert.ToBase64String(tokenBytes);
                cand.ReferalToken = referalToken;
                cand.AutoPassword = password;

               
                EmailData emailRef = new EmailData(cand.Email, "bounteous x Accolite Job Portal!", ReferalEmailBody.EmailStringBody(cand.Email, cand.ReferalToken,cand.AutoPassword));
                _emailService.SendEmail(emailRef);


                //await _dbContext.Users.AddAsync(cand);
                //await _dbContext.SaveChangesAsync();



                if (candidate.Candidate == null)
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
