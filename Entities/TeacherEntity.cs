using System.ComponentModel.DataAnnotations;

namespace DLARS.Entities
{
    public class TeacherEntity
    {
        [Key]
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string TeacherCode { get; set; }
    }
}
