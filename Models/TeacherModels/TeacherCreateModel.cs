using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.TeacherModels
{
    public class TeacherCreateModel
    {
        [Required]
        public string TeacherCode { get; set; } = string.Empty;

        [Required]
        public string TeacherName { get; set; } = string.Empty;
    }
}
