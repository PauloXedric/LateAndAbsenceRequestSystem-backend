using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.TeacherModels
{
    public class TeacherUpdateModel
    {
        [Required]
        public int TeacherId { get; set; }
        public string TeacherCode { get; set; }
        public string TeacherName { get; set; }
    }
}
