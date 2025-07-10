using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.UserAccountModels
{
    public class ResetPasswordModel
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Token { get; set; }

        [Required]
        public required string NewPassword { get; set; }
    }
}
