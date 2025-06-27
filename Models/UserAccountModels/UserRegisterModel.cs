using System.ComponentModel;
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
        [DefaultValue("User")]
        public string Role { get; set; } = "User";

        [Required]
        public string UserCode { get; set; } = string.Empty;

        [Required]
        public string LastName {  get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

    }

}
