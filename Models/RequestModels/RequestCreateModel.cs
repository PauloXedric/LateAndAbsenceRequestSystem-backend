using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.Requests
{
    public class RequestCreateModel
    {
        [Required]
        public required string StudentNumber { get; set; } 

        [Required]
        public required string StudentName { get; set; }

        [Required]
        public required string CourseYearSection { get; set; } 

        [Required]
        public required string Teacher { get; set; } 

        [Required]
        public required string SubjectCode { get; set; } 

        [Required]
        public required DateTime DateOfAbsence { get; set; } 

        [Required]
        public required DateTime DateofAttendance { get; set; }

        [Required]
        public required string ParentsCpNumber { get; set; } 

        [Required]
        public required string Reason { get; set; } 

        public bool Submitted { get; set; } = false;
    }
}
