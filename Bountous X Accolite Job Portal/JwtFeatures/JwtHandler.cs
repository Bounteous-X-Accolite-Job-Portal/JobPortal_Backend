using Bountous_X_Accolite_Job_Portal.Data;
using Bountous_X_Accolite_Job_Portal.Models;
using Bountous_X_Accolite_Job_Portal.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bountous_X_Accolite_Job_Portal.JwtFeatures
{
    public class JwtHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        private readonly ApplicationDbContext _dbContext;
        private readonly IDesignationService _designationService;
        private readonly IDistributedCache _cache;
        public JwtHandler(IConfiguration configuration, ApplicationDbContext dbContext, IDesignationService designationService, IDistributedCache cache)
        {
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
            _dbContext = dbContext;
            _designationService = designationService;
            _cache = cache;
        }
        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        public async Task<List<Claim>> GetClaims(User user)
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
                string key = $"getEmployeeById-{user.EmpId}";
                string? getEmployeeByIdFromCache = await _cache.GetStringAsync(key);

                Employee employee;
                if (string.IsNullOrWhiteSpace(getEmployeeByIdFromCache))
                {
                    employee = _dbContext.Employees.Find(user.EmpId);
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize(employee));
                }
                else
                {
                    employee = JsonSerializer.Deserialize<Employee>(getEmployeeByIdFromCache);
                }

                name = employee.FirstName;

                key = $"getDesignationById-{employee.DesignationId}";
                string? getDesignationByIdFromCache = await _cache.GetStringAsync(key);

                Designation designation;
                if (string.IsNullOrWhiteSpace(getDesignationByIdFromCache))
                {
                    designation = _dbContext.Designations.Find(employee.DesignationId);
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize(designation, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }));
                }
                else
                {
                    designation = JsonSerializer.Deserialize<Designation>(getDesignationByIdFromCache, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
                }

                role = designation.DesignationName.ToLower();

                hasPrivilege = await _designationService.HasPrivilege(designation.DesignationId);

                hasSpecialPrivilege = _designationService.HasSpecialPrivilege(role);
            }
            else
            {
                string key = $"getCandidateById-{user.CandidateId}";
                string? getCandidateByIdFromCache = await _cache.GetStringAsync(key);

                Candidate candidate;
                if (string.IsNullOrWhiteSpace(getCandidateByIdFromCache))
                {
                    candidate = _dbContext.Candidates.Find(user.CandidateId);
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize(candidate));
                }
                else
                {
                    candidate = JsonSerializer.Deserialize<Candidate>(getCandidateByIdFromCache);
                }

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
                audience: _jwtSettings["validAudience1"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }
    }
}
