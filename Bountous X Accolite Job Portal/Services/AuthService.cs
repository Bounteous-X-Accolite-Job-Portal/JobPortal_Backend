using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Models.AuthenticationViewModel;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Bountous_X_Accolite_Job_Portal.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = applicationDbContext;
        }

        public async Task<bool> Register(RegisterViewModel registerUser)
        {
            var candidate = new Candidate();
            candidate.FirstName = registerUser.FirstName;
            candidate.LastName = registerUser.LastName;
            candidate.Email = registerUser.Email;

            await _dbContext.Candidates.AddAsync(candidate);

            var user = new User();
            user.UserName = candidate.Email;
            user.Email = candidate.Email;
            user.CandidateId = candidate.CandidateId;   

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (!result.Succeeded)
            {
                _dbContext.Candidates.Remove(candidate);    
                user = null;
            }

            if (user == null)
            {
                return false;
            }

            await _dbContext.SaveChangesAsync();
            // TODO: return status codes for further error handling
            return true;
        }

        public async Task<bool> Login(UserLoginViewModel loginUser)
        {
            if(loginUser.Email == null || loginUser.Password == null)
            {
                return false;
            }

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, loginUser.RememberMe, lockoutOnFailure: false);

            return result.Succeeded;
        }
    }
}
