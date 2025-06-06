using System.ComponentModel.DataAnnotations;

namespace DLARS.Models
{
    public class UserLoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
