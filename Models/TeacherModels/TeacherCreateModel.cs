using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.TeacherModels
{
    public class TeacherCreateModel
    {
        [Required]
        public required string TeacherCode { get; set; } 

        [Required]
        public required string TeacherName { get; set; } 
    }

}
