using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.UrlTokenModels
{
    public class InvitationGenTokenModel
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string UserRole { get; set; } 
    }
}
