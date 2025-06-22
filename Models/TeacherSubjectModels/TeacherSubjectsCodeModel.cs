using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.TeacherSubjectModels
{
    public class TeacherSubjectsCodeModel
    {

        [Required]
        public string TeacherCode { get; set; }

        [Required]
        public List<string> SubjectCode { get; set; } = new();
    }
}
