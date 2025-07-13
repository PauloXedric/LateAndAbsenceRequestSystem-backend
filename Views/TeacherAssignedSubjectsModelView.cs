namespace DLARS.Views
{
    public class TeacherAssignedSubjectsModelView
    {
        public int TeacherId { get; set; }
        public string TeacherCode { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public string AssignedSubjects { get; set; } = string.Empty;
    }

}
