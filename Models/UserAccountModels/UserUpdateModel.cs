using DLARS.Enums;
using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.UserAccountModels
{
    public class UserUpdateModel
    {
        [Required]
        public required string UserCode { get; set; } 

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public UserStatus Status { get; set; } 
    }
}
