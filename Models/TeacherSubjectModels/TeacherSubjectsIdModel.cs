using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.TeacherSubjectModels
{
    public class TeacherSubjectsIdModel
    {
        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int SubjectId { get; set; }
    }

}
