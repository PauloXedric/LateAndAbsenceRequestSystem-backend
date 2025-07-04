using DLARS.Enums;
using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.RequestHistory
{
    public class RequestHistoryCreateModel
    {
       
        public DateTime ActionDate { get; set; }
        [Required]
        public int RequestId { get; set; }
        [Required]
        public RequestResult ResultId { get; set; }
        public string PerformedByUserId { get; set; } = string.Empty;
    }
}
