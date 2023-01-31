namespace AdvancedProjectMVC.Models
{
    public class CalendarEvent
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
