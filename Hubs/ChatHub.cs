﻿using AdvancedProjectMVC.Data;
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
        private static int userCount;
        private static HashSet<string> connectedUsers = new HashSet<string>();

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
            var server = await _context.Servers.FindAsync(channel.ServerId);

            var newMember = new ServerMember
            {
                ServerId = channel.ServerId,
                ApplicationUserId = user.Id
            };

            if (_context.ServerMembers.Where(s => s.ApplicationUserId == newMember.ApplicationUserId && s.ServerId == server.Id).First() == null)
            {
                _context.Add(newMember);
                _context.SaveChanges();
            }
        }

        public async Task SetConnectedUsersStatusOnline()
        {
            foreach(var user in connectedUsers)
            {
                await Clients.All.SendAsync("SetOnline", user);
            }
        }

        public override async Task OnConnectedAsync()
        {
            userCount++;

            //Send message to set user's status to online.
            string userId = Context.UserIdentifier;
            var user = await _userManager.FindByIdAsync(userId);
            string username = user.UserName;
            connectedUsers.Add(username);

            //Iterate through connected users list and set each online.
            await SetConnectedUsersStatusOnline();

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            userCount--;

            //Send message to set user's status to offline.
            string userId = Context.UserIdentifier;
            var user = await _userManager.FindByIdAsync(userId);
            string username = user.UserName;
            connectedUsers.Add(username);
            await Clients.All.SendAsync("SetOffline", username);

            await base.OnDisconnectedAsync(exception);
        }

        public string GetConnectionId()
        {
            userCount--;
            return Context.ConnectionId;
        }

        public int GetUserCount()
        {
            return userCount;
        }

        public List<string> GetConnectedUsers()
        {
            return connectedUsers.ToList();
        }
    }
}
