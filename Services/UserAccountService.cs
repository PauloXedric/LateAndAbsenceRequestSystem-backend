using DLARS.Enums;
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
        Task<(Result, ApplicationUser?)> LoginUserAccountAsync(UserLoginModel userLogin);
        Task<List<UserReadModel>> GetAllUserWithRoleAsync();
        Task<Result> UpdateUserRoleAndStatusAsync(UserUpdateModel userUpdate);
    }



    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserAccountService> _logger;


        public UserAccountService(IUserAccountRepository userAccountRepository, UserManager<ApplicationUser> userManager,
                                   ILogger<UserAccountService> logger)
        {
            _userAccountRepository = userAccountRepository;
            _userManager = userManager;
            _logger = logger;
        }
        


        public async Task<IdentityResult> RegisterAccountAsync(UserRegisterModel userAccount) 
        {
            try
            {            
                return await _userAccountRepository.AddUserAsync(userAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering new account");
                throw;
            }
        }


        public async Task<(Result, ApplicationUser?)> LoginUserAccountAsync(UserLoginModel userLogin)
        {
            try
            {
                var user = await _userAccountRepository.GetUserAccountAsync(userLogin);

                if (user == null)
                    return (Result.DoesNotExist, null);              

                var isPasswordValid = await _userManager.CheckPasswordAsync(user, userLogin.Password);

                if (!isPasswordValid)
                    return (Result.DoesNotExist, null);

                if (user.Status != UserStatus.Active)
                    return (Result.Failed, user);

                return (Result.Success, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in user: {Username}", userLogin?.Username );
                throw;
            }
        }


        public async Task<List<UserReadModel>> GetAllUserWithRoleAsync() 
        {
            try
            {
                return await _userAccountRepository.GetAllUserWithRoleAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user data");
                throw;
            }
        }


        public async Task<Result> UpdateUserRoleAndStatusAsync(UserUpdateModel userUpdate)
        {
            try 
            {
                var user = await _userAccountRepository.GetByUserCodeAsync(userUpdate.UserCode);
                if (user == null)
                {
                    _logger.LogWarning("User not found: {UserCode}", userUpdate.UserCode);
                    return Result.DoesNotExist;
                }

                var newRole = userUpdate.Role.ToString();
                var result =  await _userAccountRepository.UpdateUserRoleAndStatusAsync(user, newRole, userUpdate.Status);

                if (result == false)
                {
                   return  Result.Failed;
                }

                return Result.Success;

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occured while updating user with Code: {UserCode}", userUpdate.UserCode);
                throw;
            }
        }

    }
}
