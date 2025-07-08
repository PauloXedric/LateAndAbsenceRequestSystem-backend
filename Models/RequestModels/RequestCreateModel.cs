using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.Requests
{
    public class RequestCreateModel
    {
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
        public DateTime DateofAttendance { get; set; }

        [Required]
        public string ParentsCpNumber { get; set; } 

        [Required]
        public string Reason { get; set; } 

        public bool Submitted { get; set; } = false;
    }
}
