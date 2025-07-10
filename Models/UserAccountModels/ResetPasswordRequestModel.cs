using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.UserAccountModels
{
    public class ResetPasswordRequestModel
    {
        [Required]
        [EmailAddress]
        public required string Username { get; set; }

    }
}
