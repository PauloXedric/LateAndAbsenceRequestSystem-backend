using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.SubjectModel
{
    public class SubjectCreateModel
    {
        [Required]
        public string SubjectCode { get; set; } 

        [Required]
        public string SubjectName { get; set; } 
    }
}
