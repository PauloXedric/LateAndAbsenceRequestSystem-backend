using DLARS.Enums;
using DLARS.Models.Identity;
using DLARS.Models.UserAccountModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace DLARS.Repositories
{

    public interface IUserAccountRepository
    {
        Task<IdentityResult> AddUserAsync(UserRegisterModel userLogin);
        Task<ApplicationUser> GetUserAccountAsync(UserLoginModel userLogin);
        Task<List<UserReadModel>> GetAllUserWithRoleAsync();
        Task<bool> UpdateUserRoleAndStatusAsync(ApplicationUser user, string newRole, UserStatus newStatus);
        Task<ApplicationUser?> GetByUserCodeAsync(string userCode);
        Task<ApplicationUser?> GetByUserNameAsync(string username);
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


        public async Task<ApplicationUser> GetUserAccountAsync(UserLoginModel userLogin) 
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

                result.Add(new UserReadModel
                {
                    UserCode = user.UserCode,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Email = user.Email,                 
                    Status = user.Status.ToString(),
                    Role = roles.FirstOrDefault()
                });
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


        public async Task<ApplicationUser?> GetByUserNameAsync(string username)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

    }
}
