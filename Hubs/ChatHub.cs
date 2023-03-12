using AdvancedProjectMVC.Data;
using AdvancedProjectMVC.Models;
using AdvancedProjectMVC.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol.Core.Types;

namespace AdvancedProjectMVC.Hubs
{
    public class ChatHub :Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        //private IChatRepository _repository;



        public ChatHub(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SendMessage(string username, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", username, message);
            //_repository.AddMessage(username, message);
            
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

        public async Task AddToGroup(string groupName)
        {
            
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");

        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
