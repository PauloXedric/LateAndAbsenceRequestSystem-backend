using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.SubjectModel
{
    public class SubjectCreateModel
    {
        [Required]
        public required string SubjectCode { get; set; } 

        [Required]
        public required string SubjectName { get; set; } 
    }

}
