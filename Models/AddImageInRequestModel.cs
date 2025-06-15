using System.ComponentModel.DataAnnotations;

namespace DLARS.Models
{
    public class AddImageInRequestModel
    {
        [Required]
        public int RequestId { get; set; }

        [Required]
        public string ProofImage { get; set; }

        [Required]
        public string ParentValidImage { get; set; }

        [Required]
        public string MedicalCertificate { get; set; }
    }
}
