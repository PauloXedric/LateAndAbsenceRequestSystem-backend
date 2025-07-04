using DLARS.Enums;
using System.ComponentModel.DataAnnotations;

namespace DLARS.Entities
{
    public class RequestHistoryEntity
    {
        [Key]
        public int HistoryId { get; set; }    
        public DateTime ActionDate { get; set; }
        public int RequestId { get; set; }
        public RequestResult ResultId { get; set; }
        public string PerformedByUserId { get; set; }
    }
}
