using DLARS.Models;
using DLARS.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace DLARS.Services
{
    public interface IUserAccountService 
    {
        Task<string> GenerateTokenString(IdentityUser identityUser);
        Task<IdentityResult> RegisterAccountAsync(UserRegisterModel userAccount);
        Task<IdentityUser> LoginUserAccountAsync(UserLoginModel userLogin);
    }



    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _userManager;


        public UserAccountService(IUserAccountRepository userAccountRepository, IConfiguration config, UserManager<IdentityUser> userManager)
        {
            _userAccountRepository = userAccountRepository;
            _config = config;
            _userManager = userManager;
        }
        


        public async Task<IdentityResult> RegisterAccountAsync(UserRegisterModel userAccount) 
        {
            if (userAccount == null)
            {
                throw new ArgumentNullException("Fields cannot be empty.");
            }

            return await _userAccountRepository.AddUserAsync(userAccount);
        }


        public async Task<IdentityUser> LoginUserAccountAsync(UserLoginModel userLogin)
        {
            if (userLogin == null)
            {
                  throw new ArgumentNullException(nameof(userLogin), "User account cannot be null");
            }

             return  await _userAccountRepository.GetUserAccountAsync(userLogin);
        }



        public async Task<string> GenerateTokenString(IdentityUser identityUser) 
        {
            var roles = await _userManager.GetRolesAsync(identityUser);

            var claims = new List<Claim>
            {
              new Claim(ClaimTypes.Email, identityUser.Email),
              new Claim(ClaimTypes.NameIdentifier, identityUser.Id)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                   claims:claims,
                   expires: DateTime.Now.AddMinutes(60),
                   issuer:_config.GetSection("Jwt:Issuer").Value,
                   audience: _config.GetSection("Jwt:Audience").Value,
                   signingCredentials:signingCred);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }


    }
}
