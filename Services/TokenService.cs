using DLARS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DLARS.Services
{
    public interface ITokenService 
    {
        string GenerateUrlToken(RequestGenTokenModel request);
        Task<string> GenerateUserTokenAsync(IdentityUser identityUser);
    }


    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _userManager;

        public TokenService(IConfiguration config, UserManager<IdentityUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }


        public async Task<string> GenerateUserTokenAsync(IdentityUser identityUser)
        {
            var roles = await _userManager.GetRolesAsync(identityUser);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, identityUser.Email),
            new Claim(ClaimTypes.NameIdentifier, identityUser.Id)
        };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return BuildToken(claims, TimeSpan.FromHours(24)); 
        }


        public string GenerateUrlToken(RequestGenTokenModel request)
        {
            var claims = new List<Claim>
            {
                  new Claim("requestId", request.RequestId.ToString()),
                  new Claim("studentName", request.StudentName),
                  new Claim("studentNumber", request.StudentNumber),
                  new Claim("teacher", request.Teacher),
                  new Claim("subject", request.SubjectCode),
                  new Claim("dateOfAbsence", request.DateOfAbsence.ToString("yyyy-MM-dd")),
                  new Claim("dateOfAttendance", request.DateOfAttendance.ToString("yyyy-MM-dd")),
                  new Claim("reason", request.Reason)
            };

            return BuildToken(claims, TimeSpan.FromHours(12));
        }



        private string BuildToken(IEnumerable<Claim> claims, TimeSpan validFor)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(validFor),
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                signingCredentials: signingCred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
