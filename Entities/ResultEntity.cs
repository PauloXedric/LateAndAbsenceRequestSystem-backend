using System.ComponentModel.DataAnnotations;

namespace DLARS.Entities
{
    public class ResultEntity
    {
        [Key]
        public int ResultId { get; set; }
        public string Description { get; set; }
    }
}
