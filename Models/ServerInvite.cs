using System.Runtime.InteropServices;

namespace AdvancedProjectMVC.Models
{
    public class ServerInvite
    {
        public int Id { get; set; }
        public int serverId { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime? DateSent { get; set; }
        public string SenderUserName { get; set; }

        public Server Server { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
