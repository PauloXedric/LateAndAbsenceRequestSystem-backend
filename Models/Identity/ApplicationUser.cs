using DLARS.Enums;
using Microsoft.AspNetCore.Identity;

namespace DLARS.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserCode { get; set; } = string.Empty;

        public UserStatus Status { get; set; } = UserStatus.Inactive;

       
    }
}
