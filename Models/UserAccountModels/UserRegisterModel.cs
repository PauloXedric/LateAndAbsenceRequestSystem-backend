using DLARS.Enums;
using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.UserAccountModels
{
    public class UserRegisterModel
    {
        [Required]
        public required string UserName { get; set; } 
        [Required]  
        public required string Password { get; set; } 

        [Required]
        public UserRole Role { get; set; } 

        [Required]
        public required string UserCode { get; set; } 

        [Required]
        public required string LastName {  get; set; } 
        [Required]
        public required string FirstName { get; set; } 

    }

}
