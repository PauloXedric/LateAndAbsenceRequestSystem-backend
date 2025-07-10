using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.UserAccountModels
{
    public class UserLoginModel
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }

    }
}
