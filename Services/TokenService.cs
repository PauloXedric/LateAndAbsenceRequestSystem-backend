using DLARS.Models.Identity;
using DLARS.Models.UrlTokenModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DLARS.Services
{
    public interface ITokenService 
    {
        string GenerateUrlToken(RequestGenTokenModel request);
        Task<string> GenerateUserTokenAsync(ApplicationUser identityUser);
        string GenerateInvitationUrlToken(InvitationGenTokenModel invitation);
    }


    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration config, UserManager<ApplicationUser> userManager, ILogger<TokenService> logger)
        {
            _config = config;
            _userManager = userManager;
            _logger = logger;
        }


        public async Task<string> GenerateUserTokenAsync(ApplicationUser identityUser)
        {
            try
            {

                var roles = await _userManager.GetRolesAsync(identityUser);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, identityUser.Email ?? string.Empty),
                    new Claim(ClaimTypes.NameIdentifier, identityUser.Id)
                };

                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                return BuildToken(claims, TimeSpan.FromHours(24));
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while generating log in token user email {Email}", identityUser.Email );
                throw;
            }
        }


        public string GenerateUrlToken(RequestGenTokenModel request)
        {
            try
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

                return BuildToken(claims, TimeSpan.FromHours(24));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating UrlToken for user request");
                throw;
            }
        }


        public string GenerateInvitationUrlToken(InvitationGenTokenModel invitation)
        {
            try
            {
                var claims = new List<Claim>
            {
               new Claim("userEmail", invitation.UserEmail),
               new Claim("userRole", invitation.UserRole),
            };

                return BuildToken(claims, TimeSpan.FromHours(72));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating UrlToken for role invitation");
                throw;
            }
        }


        private string BuildToken(IEnumerable<Claim> claims, TimeSpan validFor)
        {
            var jwtKey = _config["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                _logger.LogCritical("JWT key is missing in configuration. Cannot continue.");
                throw new InvalidOperationException("Missing JWT signing key.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
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
