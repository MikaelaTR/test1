namespace AdvancedProjectMVC.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
