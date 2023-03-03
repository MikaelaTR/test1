namespace AdvancedProjectMVC.Models
{
    public class ServerMember
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public string ApplicationUserId { get; set; }

        public Server Server { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
