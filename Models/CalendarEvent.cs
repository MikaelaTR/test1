using System.ComponentModel.DataAnnotations;

namespace AdvancedProjectMVC.Models
{
    public class CalendarEvent
    {
        public int ID { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
