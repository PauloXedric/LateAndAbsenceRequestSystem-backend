using System.ComponentModel.DataAnnotations;

namespace DLARS.Models
{
    public class RequestReadModel
    {
        [Required]
        public string StudentNumber { get; set; } = string.Empty;

        [Required]
        public string StudentName { get; set; } = string.Empty;

        [Required]
        public string CourseYearSection { get; set; } = string.Empty;

        [Required]
        public string DateOfAbsence { get; set; } = string.Empty;

        [Required]
        public string DateOfAttendance { get; set; } = string.Empty;

        [Required]
        public string Reason { get; set; } = string.Empty;
    }
}
