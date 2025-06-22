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
        public string DateOfAbsence { get; set; } = string.Empty;

        [Required]
        public string DateofAttendance { get; set; } = string.Empty;

        [Required]
        public string ParentsCpNumber { get; set; } = string.Empty;

        [Required]
        public string Reason { get; set; } = string.Empty;
    }
}
