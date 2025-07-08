using DLARS.Enums;
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
        public RequestStatus StatusId { get; private set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? SecretaryInitialApprovalDate { get; set; }
        public DateTime? SecretaryProofApprovalDate { get; set; }
        public DateTime? ChairpersonApprovalDate { get; set; }
        public DateTime? DirectorApprovalDate { get; set; }
        public string? MedicalCertificate { get; set; }

        public bool Submitted { get; set; }
     
        public RequestEntity()
        {
            StatusId = RequestStatus.WaitingForFirstSecretaryApproval;
        }

        public void SetStatus(RequestStatus newStatus)
        {
            StatusId = newStatus;
        }
    } 
}
