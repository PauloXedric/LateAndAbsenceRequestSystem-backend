using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.UserAccountModels
{
    public class ResetPasswordRequestModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

    }
}
