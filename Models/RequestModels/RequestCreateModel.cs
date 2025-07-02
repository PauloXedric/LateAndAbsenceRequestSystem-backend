using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.Requests
{
    public class RequestCreateModel
    {
        [Required]
        public string StudentNumber { get; set; } = string.Empty;

        [Required]
        public string StudentName { get; set; } = string.Empty;

        [Required]
        public string CourseYearSection { get; set; } = string.Empty;

        [Required]
        public string Teacher { get; set; } = string.Empty;

        [Required]
        public string SubjectCode { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfAbsence { get; set; } 

        [Required]
        public DateTime DateofAttendance { get; set; }

        [Required]
        public string ParentsCpNumber { get; set; } = string.Empty;

        [Required]
        public string Reason { get; set; } = string.Empty;
    }
}
