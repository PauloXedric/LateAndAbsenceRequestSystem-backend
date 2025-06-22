using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.Requests
{
    public class RequestUpdateModel
    {
        [Required]
        public int RequestId { get; set; }

        [Required]
        public int StatusId { get; set; }
    }
}
