using AdvancedProjectMVC.Data;
using AdvancedProjectMVC.Models;
using AdvancedProjectMVC.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Threading.Channels;

namespace AdvancedProjectMVC.Hubs
{
    public class ChatHub :Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatHub(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SendMessage(string username, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", username, message);
            
        }

        public async Task SendMessageToGroup(string username, string message, string groupName, int channelId)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", username, message);
            var user = await _userManager.FindByNameAsync(username);
            var channel = await _context.Channels.FindAsync(channelId);

            var chatMessage = new ChatMessage
            {
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                Channel = channel,
                ChannelId = channel.Id,
                Content = message,
                DatePosted = DateTime.Now,
            };
            _context.Add(chatMessage);
            _context.SaveChanges();
        }

        public async Task AddToGroup(string groupName, string username, int channelId)
        {
            
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var user = await _userManager.FindByNameAsync(username);
            var channel = await _context.Channels.FindAsync(channelId);

            //If user does not already exist as a ServerMember, create ServerMember and add to DB.
            if (channel.Server.ServerMembers.Where(s => s.ApplicationUserId == user.Id) == null)
            {
                var member = new ServerMember
                {
                    Server = channel.Server,
                    ServerId = channel.ServerId,
                    ApplicationUser = user,
                    ApplicationUserId = user.Id
                };
                _context.ServerMembers.Add(member);
                _context.SaveChanges();
            }
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
