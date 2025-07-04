using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.UrlTokenModels
{
    public class RequestGenTokenModel
    {
        [Required]
        public int RequestId { get; set; }
        [Required]
        public string StudentNumber { get; set; }
        [Required]
        public string StudentName { get; set; }
        [Required]
        public string CourseYearSection { get; set; }
        [Required]
        public string Teacher { get; set; }
        [Required]
        public string SubjectCode { get; set; }
        [Required]
        public DateTime DateOfAbsence { get; set; }
        [Required]
        public DateTime DateOfAttendance { get; set; }
        [Required]
        public string Reason { get; set; } 
    }
}
