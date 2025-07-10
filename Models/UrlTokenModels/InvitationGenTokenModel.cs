using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.UrlTokenModels
{
    public class InvitationGenTokenModel
    {
        [Required]
        public required string UserEmail { get; set; }
        [Required]
        public required string UserRole { get; set; } 
    }
}
