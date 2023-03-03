using Microsoft.Build.Framework;

namespace AdvancedProjectMVC.Models
{
    public class Channel
    {
        public int Id { get; set; }
        [Required]
        public string ChannelName { get; set; }

        [Required]
        public int ServerId { get; set; }
        public Server Server { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
