using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
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
        public JwtHandler(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
            _dbContext = dbContext;
        }
        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        public List<Claim> GetClaims(User user)
        {

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
        };

            claims.Add(new Claim(type: "IsEmployee", value: (user.EmpId == null ? false : true).ToString(), ClaimValueTypes.Boolean));
            claims.Add(new Claim(type: "Id", value: (user.EmpId != null ? user.EmpId : user.CandidateId).ToString()));

            var role = "user";
            if (user.EmpId != null)
            {
                var designationId = _dbContext.Employees.Find(user.EmpId).DesignationId;
                role = _dbContext.Designations.Find(designationId).DesignationName.ToLower();
            }

            claims.Add(new Claim(type: "Role", value: role));

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
