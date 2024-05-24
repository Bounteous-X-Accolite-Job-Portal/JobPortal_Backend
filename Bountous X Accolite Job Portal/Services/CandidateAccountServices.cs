using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModels;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Bountous_X_Accolite_Job_Portal.Models.EMAIL;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class CandidateAccountServices : ICandidateAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IDistributedCache _cache;
        public CandidateAccountServices(UserManager<User> userManager, ApplicationDbContext applicationDbContext, IEmailService emailService, IConfiguration configuration, IDistributedCache cache)
        {
            _userManager = userManager;
            _dbContext = applicationDbContext;
            _emailService = emailService;
            _config = configuration;
            _cache = cache;
        }

        public async Task<CandidateResponseViewModel> GetCandidateById(Guid CandidateId)
        {
            CandidateResponseViewModel response = new CandidateResponseViewModel();

            string key = $"getCandidateById-{CandidateId}";
            string? getCandidateByIdFromCache = await _cache.GetStringAsync(key);

            Candidate candidate;
            if (string.IsNullOrEmpty(getCandidateByIdFromCache))
            {
                candidate = _dbContext.Candidates.Find(CandidateId);
                if (candidate == null)
                {
                    response.Status = 404;
                    response.Message = "Please enter a valid candidateId.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(candidate));
            }
            else
            {
                candidate = JsonSerializer.Deserialize<Candidate>(getCandidateByIdFromCache);
            }

            response.Status = 200;
            response.Message = "Successfully retrieved candidate with given Id.";
            response.Candidate = new CandidateViewModel(candidate);
            return response;
        }

        public async Task<CandidateResponseViewModel> Register(CandidateRegisterViewModel registerUser)
        {
            CandidateResponseViewModel response = new CandidateResponseViewModel();

            string key = $"getUserByEmail-{registerUser.Email}";
            string? getUserByEmailFromCache = await _cache.GetStringAsync(key);

            User checkUserWhetherExist;
            if (string.IsNullOrEmpty(getUserByEmailFromCache))
            {
                checkUserWhetherExist = _dbContext.Users.Where(item => item.Email == registerUser.Email).FirstOrDefault();
                if (checkUserWhetherExist != null)
                {
                    response.Status = 409;
                    response.Message = "This email is already registered with us. Please Login.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(checkUserWhetherExist));
            }
            else
            {
                checkUserWhetherExist = JsonSerializer.Deserialize<User>(getUserByEmailFromCache);
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

        public async Task<CandidateResponseViewModel> UpdateCandidateProfile(UpdateCandidateViewModel updatedCandidate)
        {
            CandidateResponseViewModel response = new CandidateResponseViewModel();

            string key = $"getCandidateById-{updatedCandidate.CandidateId}";
            string? getCandidateByIdFromCache = await _cache.GetStringAsync(key);

            Candidate candidate;
            if (string.IsNullOrEmpty(getCandidateByIdFromCache))
            {
                candidate = _dbContext.Candidates.Find(updatedCandidate.CandidateId);
                if (candidate == null)
                {
                    response.Status = 404;
                    response.Message = "Please enter a valid candidateId.";
                    return response;
                }

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(candidate));
            }
            else
            {
                candidate = JsonSerializer.Deserialize<Candidate>(getCandidateByIdFromCache);
            }

            candidate.FirstName = updatedCandidate.FirstName;
            candidate.LastName = updatedCandidate.LastName;
            candidate.Phone = updatedCandidate.Phone;
            candidate.AddressLine1 = updatedCandidate.AddressLine1;
            candidate.City = updatedCandidate.City;
            candidate.State = updatedCandidate.State;
            candidate.Country = updatedCandidate.Country;
            candidate.ZipCode = updatedCandidate.ZipCode;

            _dbContext.Candidates.Update(candidate);
            await _dbContext.SaveChangesAsync();

            await _cache.RemoveAsync($"getCandidateById-{candidate.CandidateId}");

            response.Status = 200;
            response.Message = "Successfully updated the candidate.";
            response.Candidate = new CandidateViewModel(candidate);
            return response;
        }
    }
}
