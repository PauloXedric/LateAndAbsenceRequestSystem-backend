using System.ComponentModel.DataAnnotations;

namespace DLARS.Entities
{
    public class StatusEntity
    {
        [Key]
        public int StatusId { get; set; }
        public string Description { get; set; }
    }
}
