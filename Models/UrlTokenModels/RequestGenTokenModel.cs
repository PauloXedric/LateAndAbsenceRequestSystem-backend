using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.UrlTokenModels
{
    public class RequestGenTokenModel
    {
        [Required]
        public int RequestId { get; set; }
        [Required]
        public required string StudentNumber { get; set; }
        [Required]
        public required string StudentName { get; set; }
        [Required]
        public required string CourseYearSection { get; set; }
        [Required]
        public  required string Teacher { get; set; }
        [Required]
        public required string SubjectCode { get; set; }
        [Required]
        public DateTime DateOfAbsence { get; set; }
        [Required]
        public DateTime DateOfAttendance { get; set; }
        [Required]
        public required string Reason { get; set; } 
    }
}
