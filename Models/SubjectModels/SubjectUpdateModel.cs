using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.SubjectModels
{
    public class SubjectUpdateModel
    {
        [Required]
        public int SubjectId { get; set; }

        [Required]
        public  required string SubjectCode { get; set; }

        [Required]
        public required string SubjectName { get; set; } 
    }

}
