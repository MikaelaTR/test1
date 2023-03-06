using AdvancedProjectMVC.Data;
using AdvancedProjectMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

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
            var user = await _userManager.FindByNameAsync(username);

            var chatMessage = new ChatMessage
            {
                ApplicationUserId = user.Id,
                Content = message,
                DatePosted = DateTime.Now,
            };

            _context.Add(chatMessage);
            _context.SaveChanges();
        }
    }
}
