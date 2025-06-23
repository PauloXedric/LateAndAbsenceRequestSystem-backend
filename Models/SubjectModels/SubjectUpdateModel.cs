using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.SubjectModels
{
    public class SubjectUpdateModel
    {
        [Required]
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
    }
}
