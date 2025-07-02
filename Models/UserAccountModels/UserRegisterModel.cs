using DLARS.Enums;
using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.UserAccountModels
{
    public class UserRegisterModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]  
        public string Password { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } 

        [Required]
        public string UserCode { get; set; } = string.Empty;

        [Required]
        public string LastName {  get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

    }

}
