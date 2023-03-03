using Microsoft.Build.Framework;

namespace AdvancedProjectMVC.Models
{
    public class Server
    {
        public int ServerId { get; set; }
        [Required]
        public string ServerName { get; set; }

        public ICollection<Channel> Channels { get; set;}
        public ICollection<ApplicationUser> ApplicationUsers { get; set;}
    }
}
