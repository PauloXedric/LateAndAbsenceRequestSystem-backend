using System.ComponentModel.DataAnnotations;

namespace DLARS.Models
{
    public class TeacherSubjectsCodeModel
    {

        [Required]
        public string TeacherCode { get; set; }

        [Required]
        public string SubjectCode { get; set; }
    }
}
