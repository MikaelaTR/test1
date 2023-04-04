using Microsoft.Build.Framework;

namespace AdvancedProjectMVC.Models
{
    public class SharedFile
    {
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }
        public string? FilePath { get; set; }
        public int? ServerID { get; set; }
        public int? ChannelID { get; set; }
        [Required]
        public string? ApplicationUserID { get; set; }

        public string? CreatorID { get; set; }
        public string? ServerName { get; set; }
        public string? ChannelName { get; set; }
        public string? TempFile { get; set; }
        public string? DownloadURL { get; set; }
    }
}
