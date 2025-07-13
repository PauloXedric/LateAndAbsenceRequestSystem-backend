
namespace DLARS.Models.Requests
{
    public class RequestReadModel
    {
        public int RequestId { get; set; }

        public string StudentNumber { get; set; } = string.Empty;

        public string StudentName { get; set; } = string.Empty;

        public string CourseYearSection { get; set; } = string.Empty;

        public string Teacher { get; set; } = string.Empty;

        public string SubjectCode { get; set; } = string.Empty;

        public DateTime DateOfAbsence { get; set; } 

        public DateTime DateOfAttendance { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string ProofImage { get; set; } = string.Empty;

        public string ParentValidImage { get; set; } = string.Empty;

        public string? MedicalCertificate { get; set; }
    }

}
