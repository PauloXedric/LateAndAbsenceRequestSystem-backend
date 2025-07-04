using DLARS.Enums;
using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.UserAccountModels
{
    public class UserRegisterModel
    {
        [Required]
        public string UserName { get; set; } 
        [Required]  
        public string Password { get; set; } 

        [Required]
        public UserRole Role { get; set; } 

        [Required]
        public string UserCode { get; set; } 

        [Required]
        public string LastName {  get; set; } 
        [Required]
        public string FirstName { get; set; } 

    }

}
