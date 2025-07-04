using System.ComponentModel.DataAnnotations;

namespace DLARS.Models.Requests
{
    public class RequestReadModel
    {
        public int RequestId { get; set; }
        public string StudentNumber { get; set; } 
        public string StudentName { get; set; } 
        public string CourseYearSection { get; set; } 
        public string Teacher { get; set; } 
        public string SubjectCode { get; set; } 
        public DateTime DateOfAbsence { get; set; } 
        public DateTime DateOfAttendance { get; set; }
        public string Reason { get; set; } 
        public string ProofImage { get; set; } 
        public string ParentValidImage { get; set; } 
        public string? MedicalCertificate { get; set; }
    }
}
