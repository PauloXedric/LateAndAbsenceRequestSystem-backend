namespace DLARS.Views
{
    public class RequestResultHistoryModelView
    {
        public int HistoryId { get; set; }
        public DateTime ActionDate { get; set; }
        public string StudentNumber { get; set; }
        public string StudentName { get; set; }
        public string CourseYearSection { get; set; }
        public string Description { get; set; }
        public string PerformedByUser { get;  set; }
    }
}
