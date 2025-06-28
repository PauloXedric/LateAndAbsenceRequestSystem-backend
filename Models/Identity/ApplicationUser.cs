using Microsoft.AspNetCore.Identity;

namespace DLARS.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserCode { get; set; } = string.Empty;

        public string Status { get; private set; }

        public ApplicationUser() 
        {
            Status = "Inactive";
        }
    }
}
