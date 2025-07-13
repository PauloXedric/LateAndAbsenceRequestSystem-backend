namespace DLARS.Views
{
    public class RequestResultHistoryModelView
    {
        public int HistoryId { get; set; }

        public DateTime ActionDate { get; set; }

        public string StudentNumber { get; set; } = string.Empty;

        public string StudentName { get; set; } = string.Empty;

        public string CourseYearSection { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string PerformedByUser { get; set; } = string.Empty;
    }   

}
