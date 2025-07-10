using DLARS.Enums;
using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.Requests
{
    public class AddImageReceivedInRequestModel
    {
        [Required]
        public int RequestId { get; set; }

        [Required]
        public required IFormFile ProofImage { get; set; }

        [Required]
        public required IFormFile ParentValidImage { get; set; }

        public IFormFile? MedicalCertificate { get; set; }

        [Required]
        public RequestStatus StatusId { get; set; }

        public bool Submitted { get; set; } = true;
    }
}
