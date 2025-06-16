using System.ComponentModel.DataAnnotations;

namespace DLARS.Models
{
    public class AddImageUploadInRequestModel
    {
        [Required]
        public int RequestId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public string ProofImage { get; set; }

        [Required]
        public string ParentValidImage { get; set; }

        public string? MedicalCertificate { get; set; }
    }
}
