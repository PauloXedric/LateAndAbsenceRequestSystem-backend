using System.ComponentModel.DataAnnotations;

namespace DLARS.Entities
{
    public class RequestEntity
    {
        [Key]
        public int RequestId { get; set; }
        public string StudentNumber { get; set; }
        public string StudentName { get; set; }
        public string CourseYearSection { get; set; }
        public string Teacher { get; set; }
        public string SubjectCode { get; set; }
        public DateTime DateOfAbsence { get; set; }
        public DateTime DateofAttendance { get; set; }
        public string ParentsCpNumber { get; set; }
        public string Reason { get; set; }
        public string? ProofImage { get; set; }
        public string? ParentValidImage { get; set; }
        public int StatusId { get; set; }
        public string? CreatedOn { get; set; }
        public string? ModifiedOn { get; set; }
        public string? SecretaryInitialApprovalDate { get; set; }
        public string? SecretaryProofApprovalDate { get; set; }
        public string? ChairpersonApprovalDate { get; set; }
        public string? DirectorApprovalDate { get; set; }
    }
}
