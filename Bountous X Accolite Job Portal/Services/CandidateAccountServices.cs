using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel.CandidateViewModel;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class CandidateAccountServices : ICandidateAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public CandidateAccountServices(UserManager<User> userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _dbContext = applicationDbContext;
        }

        public async Task<CandidateResponseViewModel> Register(CandidateRegisterViewModel registerUser)
        {
            CandidateResponseViewModel response = new CandidateResponseViewModel();

            var checkUserWhetherExist = _dbContext.Users.Where(item => item.Email == registerUser.Email).ToList();
            if(checkUserWhetherExist.Count != 0)
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

            response.Status = 200;
            response.Message = "Successfully created Candidate.";
            response.Candidate = new CandidateViewModel(candidate);
            return response;
        }
    }
}
