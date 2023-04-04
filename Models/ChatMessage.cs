using System.ComponentModel.DataAnnotations;

namespace AdvancedProjectMVC.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public int ChannelId { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime DatePosted { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Channel Channel { get; set; }
    }
}
