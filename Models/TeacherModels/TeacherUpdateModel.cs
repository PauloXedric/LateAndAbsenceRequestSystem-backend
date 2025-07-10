using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.TeacherModels
{
    public class TeacherUpdateModel
    {
        [Required]
        public int TeacherId { get; set; }

        [Required]
        public required string TeacherCode { get; set; }

        [Required]
        public required string TeacherName { get; set; }
    }
}
