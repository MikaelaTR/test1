namespace AdvancedProjectMVC.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int ChannelId { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Channel Channel { get; set; }
    }
}
