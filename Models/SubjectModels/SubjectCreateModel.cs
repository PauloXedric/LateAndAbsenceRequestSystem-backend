using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.SubjectModel
{
    public class SubjectCreateModel
    {
        [Required]
        public string SubjectCode { get; set; } = string.Empty;

        [Required]
        public string SubjectName { get; set; } = string.Empty;
    }
}
