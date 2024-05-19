using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bountous_X_Accolite_Job_Portal.JwtFeatures
{
    public class JwtHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        private readonly ApplicationDbContext _dbContext;
        private readonly IDesignationService _designationService;
        public JwtHandler(IConfiguration configuration, ApplicationDbContext dbContext, IDesignationService designationService)
        {
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
            _dbContext = dbContext;
            _designationService = designationService;
        }
        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        public List<Claim> GetClaims(User user)
        {

            var claims = new List<Claim>();

            claims.Add(new Claim(type: "Email", value: user.Email));
            claims.Add(new Claim(type: "IsEmployee", value: (user.EmpId == null ? false : true).ToString(), ClaimValueTypes.Boolean));
            claims.Add(new Claim(type: "Id", value: (user.EmpId != null ? user.EmpId : user.CandidateId).ToString()));

            var role = "user";
            string name = "";
            bool hasPrivilege = false;
            bool hasSpecialPrivilege = false;
            if (user.EmpId != null)
            {
                var employee = _dbContext.Employees.Find(user.EmpId);
                name = employee.FirstName;
                role = _dbContext.Designations.Find(employee.DesignationId).DesignationName.ToLower();

                hasPrivilege = _designationService.HasPrivilege(role);

                hasSpecialPrivilege = _designationService.HasSpecialPrivilege(role);
            }
            else
            {
                var candidate = _dbContext.Candidates.Find(user.CandidateId);
                name = candidate.FirstName;
            }

            claims.Add(new Claim(type: "Role", value: role));
            claims.Add(new Claim(type: "Name", value: name));
            claims.Add(new Claim(type: "HasPrivilege", value: hasPrivilege.ToString(), ClaimValueTypes.Boolean));
            claims.Add(new Claim(type: "HasSpecialPrivilege", value: hasSpecialPrivilege.ToString(), ClaimValueTypes.Boolean));

            return claims;
        }
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings["validIssuer"],
                audience: _jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }
    }
}
