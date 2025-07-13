using DLARS.Enums;
using DLARS.Models.Identity;

namespace DLARS.Models.UserAccountModels
{
    public class UserReadModel
    {
        public string UserCode { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;


        public static UserReadModel FromUser(ApplicationUser user, IList<string> roles)
        {
            return new UserReadModel
            {
                UserCode = user.UserCode,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Email = user.Email ?? string.Empty,
                Status = user.Status.ToString(),
                Role = roles.FirstOrDefault() ?? string.Empty
            };
        }

    }
}
