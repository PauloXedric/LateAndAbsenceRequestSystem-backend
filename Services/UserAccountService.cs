using DLARS.Enums;
using DLARS.Models.Identity;
using DLARS.Models.UserAccountModels;
using DLARS.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DLARS.Services
{

    public interface IUserAccountService 
    {
        Task<IdentityResult> RegisterAccountAsync(UserRegisterModel userAccount);
        Task<(Result, ApplicationUser?)> LoginUserAccountAsync(UserLoginModel userLogin);
        Task<List<UserReadModel>> GetAllUserWithRoleAsync();
        Task<Result> UpdateUserRoleAndStatusAsync(UserUpdateModel userUpdate);
        Task<bool> CheckUserExistenceAsync(string username);
        Task<string?> GenerateResetPasswordTokenAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
        Task<bool> ValidateResetTokenAsync(ResetTokenValidationModel resetToken);
        Task<Result> DeleteAccountByEmailAsync(string email);
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


        public async Task<bool> CheckUserExistenceAsync(string username)
        {
            try
            {
                bool userExist = await _userAccountRepository.GetByUserNameAsync(username);

                if (!userExist)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking username {UserName}", username);
                throw;
            }
        }


        public async Task<string?> GenerateResetPasswordTokenAsync(string email)
        {
            try
            {
                return await _userAccountRepository.GenerateResetPasswordTokenAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating reset password token for user: {Email}", email);
                throw;
            }
        }


        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                return await _userAccountRepository.ResetPasswordAsync(email, token, newPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while reseting password of user: {Email}", email);
                throw;
            }
        }


        public async Task<bool> ValidateResetTokenAsync(ResetTokenValidationModel resetToken)
        {
            try 
            {
                return await _userAccountRepository.ValidateResetPasswordTokenAsync(resetToken);
            } 
            catch (Exception ex)
            { 
                _logger.LogError(ex, "Error occured while validating reset password token");
                throw; 
            }
        }


        public async Task<Result> DeleteAccountByEmailAsync(string email)
        {
            try
            {
                var deleted = await _userAccountRepository.DeleteUserAsync(email);

                if (deleted == false)
                {
                    return Result.Failed;
                }

                return Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user account {}", email);
                throw;
            }
        }



    }
}
