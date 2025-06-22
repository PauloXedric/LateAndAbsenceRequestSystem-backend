using DLARS.Models.UserAccountModels;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace DLARS.Repositories
{

    public interface IUserAccountRepository
    {
        Task<IdentityResult> AddUserAsync(UserRegisterModel userLogin);
        Task<IdentityUser> GetUserAccountAsync(UserLoginModel userLogin);
    }


    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _userRole;

        public UserAccountRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> userRole)
        {
            _userManager = userManager;
            _userRole = userRole;
        }


        public async Task<IdentityResult> AddUserAsync(UserRegisterModel userAccount)
        {
            var identityUser = new IdentityUser
            {
                UserName = userAccount.UserName,
                Email = userAccount.UserName
            };

            var newUser =  await _userManager.CreateAsync(identityUser, userAccount.Password);

            if (newUser.Succeeded) 
            {
                var role =  userAccount.Role;

                var roleExists = await _userRole.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await _userRole.CreateAsync(new IdentityRole(role));
                }
                await _userManager.AddToRoleAsync(identityUser, role);
            }

            return newUser;
        }


        public async Task<IdentityUser> GetUserAccountAsync(UserLoginModel userLogin) 
        {
            var identifyUser = await _userManager.FindByNameAsync(userLogin.Username);

            if (identifyUser is null) 
            {
                return null;
            }

            return identifyUser;
        }



    }
}
