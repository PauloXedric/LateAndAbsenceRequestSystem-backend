using DLARS.Enums;
using DLARS.Models.Identity;
using DLARS.Models.UserAccountModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace DLARS.Repositories
{

    public interface IUserAccountRepository
    {
        Task<IdentityResult> AddUserAsync(UserRegisterModel userLogin);
        Task<ApplicationUser?> GetUserAccountAsync(UserLoginModel userLogin);
        Task<List<UserReadModel>> GetAllUserWithRoleAsync();
        Task<bool> UpdateUserRoleAndStatusAsync(ApplicationUser user, string newRole, UserStatus newStatus);
        Task<ApplicationUser?> GetByUserCodeAsync(string userCode);
        Task<bool> GetByUserNameAsync(string username);
        Task<string?> GenerateResetPasswordTokenAsync(string email);
        Task<bool> ValidateResetPasswordTokenAsync(ResetTokenValidationModel resetModel);
        Task<bool> ResetPasswordAsync(string email, string encodedToken, string newPassword);
        Task<bool> DeleteUserAsync(string userAccount);
    }


    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _userRole;

        public UserAccountRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> userRole)
        {
            _userManager = userManager;
            _userRole = userRole;
        }


        public async Task<IdentityResult> AddUserAsync(UserRegisterModel userAccount)
        {
            var identityUser = new ApplicationUser
            {
                UserName = userAccount.UserName,
                Email = userAccount.UserName,
                UserCode = userAccount.UserCode,
                LastName = userAccount.LastName,
                FirstName = userAccount.FirstName,
            };

            var newUser =  await _userManager.CreateAsync(identityUser, userAccount.Password);

            if (newUser.Succeeded) 
            {
                var role =  userAccount.Role.ToString();

                var roleExists = await _userRole.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await _userRole.CreateAsync(new IdentityRole(role));
                }
                await _userManager.AddToRoleAsync(identityUser, role);
            }

            return newUser;
        }


        public async Task<ApplicationUser?> GetUserAccountAsync(UserLoginModel userLogin) 
        {
            var identifyUser = await _userManager.FindByNameAsync(userLogin.Username);

            if (identifyUser is null) 
            {
                return null;
            }

            return identifyUser;
        }


        public async Task<List<UserReadModel>> GetAllUserWithRoleAsync() 
        {
            var users = _userManager.Users.ToList();
            var result = new List<UserReadModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains(UserRole.Developer.ToString()))
                    continue;

                result.Add(UserReadModel.FromUser(user, roles));
            }

            return result;
        }


        public async Task<bool> UpdateUserRoleAndStatusAsync(ApplicationUser user, string newRole, UserStatus newStatus)
        {
            user.Status = newStatus;

            var updateResult = await _userManager.UpdateAsync(user);

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var addResult = await _userManager.AddToRoleAsync(user, newRole);

            bool roleUpdated = removeResult.Succeeded && addResult.Succeeded;

            return updateResult.Succeeded || roleUpdated;
        }


        public async Task<ApplicationUser?> GetByUserCodeAsync(string userCode)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserCode == userCode);
        }


        public async Task<bool> GetByUserNameAsync(string username)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName == username);
        }


        public async Task<string?> GenerateResetPasswordTokenAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null) return null;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return WebUtility.UrlEncode(token);
        }


        public async Task<bool> ValidateResetPasswordTokenAsync(ResetTokenValidationModel resetModel)
        {
            var user = await GetUserByEmailAsync(resetModel.Email);
            if (user == null)
                return false;

            var decodedToken = Uri.UnescapeDataString(resetModel.Token);

            return await _userManager.VerifyUserTokenAsync(
                         user,
                         _userManager.Options.Tokens.PasswordResetTokenProvider,
                         UserManager<ApplicationUser>.ResetPasswordTokenPurpose,
                         decodedToken);
        }


        public async Task<bool> ResetPasswordAsync(string email, string encodedToken, string newPassword)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null)
                return false;

            var token = WebUtility.UrlDecode(encodedToken);
            token = token.Replace(" ", "+");
            var  result =  await _userManager.ResetPasswordAsync(user, token, newPassword);

            return result.Succeeded;
        }


        public async Task<bool> DeleteUserAsync(string userAccount)
        {
            var user = await GetUserByEmailAsync(userAccount);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }


        private async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }



    }
}
