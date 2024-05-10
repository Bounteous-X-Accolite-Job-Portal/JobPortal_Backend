using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using Microsoft.AspNetCore.Mvc;
using Bountous_X_Accolite_Job_Portal.Models.EMAIL;
using Azure.Core;
using MimeKit.Text;
using MimeKit;
using static Org.BouncyCastle.Math.EC.ECCurve;
using MailKit.Net.Smtp;


namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class CandidateAccountServices : ICandidateAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;



        public CandidateAccountServices(UserManager<User> userManager, ApplicationDbContext applicationDbContext, IEmailService emailService, IConfiguration configuration)

        {
            _userManager = userManager;
            _dbContext = applicationDbContext;
            _emailService = emailService;
            _config = configuration;
        }

        public CandidateResponseViewModel GetCandidateById(Guid CandidateId)
        {
            CandidateResponseViewModel response = new CandidateResponseViewModel();

            var candidate = _dbContext.Candidates.Find(CandidateId);
            if(candidate == null)
            {
                response.Status = 404;
                response.Message = "Please enter a valid candidateId.";
                return response;
            }

            response.Status = 200;
            response.Message = "Successfully retrieved candidate with given Id.";
            response.Candidate = new CandidateViewModel(candidate);
            return response;
        }

        public async Task<CandidateResponseViewModel> Register(CandidateRegisterViewModel registerUser)
        {
            CandidateResponseViewModel response = new CandidateResponseViewModel();

            var checkUserWhetherExist = _dbContext.Users.Where(item => item.Email == registerUser.Email).ToList();
            if (checkUserWhetherExist.Count != 0)
            {
                response.Status = 409;
                response.Message = "This email is already registered with us. Please login.";
                return response;
            }

            var candidate = new Candidate();
            candidate.FirstName = registerUser.FirstName;
            candidate.LastName = registerUser.LastName;
            candidate.Email = registerUser.Email;
            

            await _dbContext.Candidates.AddAsync(candidate);
            await _dbContext.SaveChangesAsync();

            if (candidate == null)
            {
                response.Status = 500;
                response.Message = "Unable to create User, please try again.";
                return response;
            }

            var user = new User();
            user.UserName = candidate.Email;
            user.Email = candidate.Email;
            user.CandidateId = candidate.CandidateId;
            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (!result.Succeeded)
            {
                _dbContext.Candidates.Remove(candidate);
                await _dbContext.SaveChangesAsync();

                response.Status = 500;
                response.Message = "Unable to create User, please try again.";
                return response;
            }

            //ConfirmEMail

            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            ////onsole.WriteLine(token);
            //_dbContext.Update(user);
            //await _dbContext.SaveChangesAsync();



            response.Status = 200;
            response.Message = "Successfully created Candidate.";
            response.Candidate = new CandidateViewModel(candidate);
            return response;
        }
        public void SendConfirmEmail(EmailData request)
        {
            var email = new MimeMessage();
            var from = _config["EmailSettings:From"];
            email.From.Add(new MailboxAddress("Job Portal", from));
            email.To.Add(new MailboxAddress(request.To, request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html)
            { Text = string.Format(request.Body) };

            using var smtp = new SmtpClient();
            {
                try
                {
                    smtp.Connect(_config["EmailSettings:SmtpServer"], 465, true);
                    var temp = _config.GetSection("EmailSettings:From").Value;
                    var temp2 = _config.GetSection("EmailSettings:EmailPassword").Value;

                    smtp.Authenticate(_config.GetSection("EmailSettings:From").Value, _config.GetSection("EmailSettings:EmailPassword").Value);
                    smtp.Send(email);
                }
                catch (Exception ex)
                {
                    throw;

                }
                finally
                {
                    smtp.Disconnect(true);
                    smtp.Dispose();
                }
            }



        }

    }
}
