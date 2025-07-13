using DLARS.Enums;
using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.Requests
{
    public class AddImageUploadInRequestModel
    {
        [Required]
        public int RequestId { get; set; }

        [Required]
        public RequestStatus StatusId { get; set; }

        [Required]
        public required string ProofImage { get; set; }

        [Required]
        public required string ParentValidImage { get; set; }

        public string? MedicalCertificate { get; set; }

        [Required]
        public bool Submitted { get; set; }
    }

}
