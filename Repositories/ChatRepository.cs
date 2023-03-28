using AdvancedProjectMVC.Data;
using AdvancedProjectMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProjectMVC.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async void AddMessage(string username, string message)
        {
            //Channel channel = await _context.Channels.FindAsync(channelId);
            //string groupName = channel.ChannelName;
            ApplicationUser? user = await _userManager.FindByNameAsync(username);


            var chatMessage = new ChatMessage
            {
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                //Channel = channel,
                //ChannelId = channel.Id,
                Content = message,
                DatePosted = DateTime.Now,
            };

            _context.Add(chatMessage);
            _context.SaveChanges();
        }
    }
}
