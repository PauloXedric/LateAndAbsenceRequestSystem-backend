using System.ComponentModel.DataAnnotations;

namespace DLARS.Models
{
    public class TeacherModel
    {
        [Required]
        public string TeacherCode { get; set; } = string.Empty;

        [Required]
        public string TeacherName { get; set; } = string.Empty;
    }
}
