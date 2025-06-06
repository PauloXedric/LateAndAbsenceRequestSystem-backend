using System.ComponentModel.DataAnnotations;

namespace DLARS.Models
{
    public class SubjectModel
    {
        [Required]
        public string SubjectCode { get; set; } = string.Empty;

        [Required]
        public string SubjectName { get; set; } = string.Empty;
    }
}
