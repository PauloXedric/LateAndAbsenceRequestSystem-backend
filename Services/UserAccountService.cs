using DLARS.Models.Identity;
using DLARS.Models.UserAccountModels;
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
        Task<IdentityResult> RegisterAccountAsync(UserRegisterModel userAccount);
        Task<ApplicationUser> LoginUserAccountAsync(UserLoginModel userLogin);
    }



    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;


        public UserAccountService(IUserAccountRepository userAccountRepository, IConfiguration config, UserManager<ApplicationUser> userManager)
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


        public async Task<ApplicationUser> LoginUserAccountAsync(UserLoginModel userLogin)
        {
            if (userLogin == null)
            {
                  throw new ArgumentNullException(nameof(userLogin), "User account cannot be null");
            }

             var user =  await _userAccountRepository.GetUserAccountAsync(userLogin);

             if (user == null)
                return null;
   
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, userLogin.Password);
            return isPasswordValid ? user : null;
        }


    }
}
