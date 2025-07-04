using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.TeacherModels
{
    public class TeacherCreateModel
    {
        [Required]
        public string TeacherCode { get; set; } 

        [Required]
        public string TeacherName { get; set; } 
    }
}
