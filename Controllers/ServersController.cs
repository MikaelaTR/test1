﻿using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvancedProjectMVC.Data;
using AdvancedProjectMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AdvancedProjectMVC.Controllers
{
    [Authorize]
    public class ServersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Servers
        public async Task<IActionResult> Index()
        {
            // return View();
            //Get list of servers the user belongs to by querying ServerMembers for 
            //serverIds
            List<Server> servers = new List<Server>();
            List<ServerMember> memberships = new List<ServerMember>();

            var user = _userManager.GetUserAsync(User).Result;
            memberships =  await _context.ServerMembers.Where(m => m.ApplicationUser == user).ToListAsync();
            foreach(var m in memberships)
            {
                var server = await _context.Servers.Where(s => s.Id == m.ServerId).FirstAsync();
                servers.Add(server);
            }
            
            return _context.Servers != null ?
                        View(servers.Distinct()) :
                        Problem("Entity set 'ApplicationDbContext.Servers'  is null.");
        }

        /*public async Task<IActionResult> _ServerListSidebar()
        {
            return _context.Servers != null
                ? PartialView(await _context.Servers.ToListAsync())
                : Problem("Entity set 'ApplicationDbContext.Servers'  is null.");
        }*/

        // GET: Servers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Servers == null)
            {
                return NotFound();
            }

            var server = await _context.Servers
                .Include(x => x.Channels)
                .Include(x => x.ServerMembers)
                .ThenInclude(s => s.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (server == null)
            {
                return NotFound();
            }

            return View(server);
        }

        // GET: Servers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Servers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServerId,ServerName")] Server server)
        {
           // if (ModelState.IsValid)
            //{
            _context.Add(server);
            await _context.SaveChangesAsync();

            Channel channel = new Channel
            {
                ChannelName = "General",
                Server = server,
            };
            _context.Channels.Add(channel);

            await _context.SaveChangesAsync();
            var user = await _userManager.GetUserAsync(User);

            //Add user as serverMember
            ServerMember serverMember = new ServerMember();
            serverMember.ApplicationUserId = user.Id;
            serverMember.ServerId = server.Id;
            await new ServerMembersController(_context).Create(serverMember);

            return RedirectToAction(nameof(Index));
            //}
            //return View(server);
        }

        //For creation of DM's, adds both users to server
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDM([Bind("ServerId,ServerName")] Server server, string userId)
        {
            // if (ModelState.IsValid)
            //{
            _context.Add(server);
            await _context.SaveChangesAsync();

            Channel channel = new Channel
            {
                ChannelName = "General",
                Server = server,
            };
            _context.Channels.Add(channel);

            await _context.SaveChangesAsync();
            var user = await _userManager.GetUserAsync(User);

            //Add user as serverMember
            ServerMember serverMember = new ServerMember();
            serverMember.ApplicationUserId = user.Id;
            serverMember.ServerId = server.Id;

            //Add user as serverMember
            ServerMember serverMember2 = new ServerMember();
            serverMember2.ApplicationUserId = userId;
            serverMember2.ServerId = server.Id;
            await new ServerMembersController(_context).Create(serverMember);
            await new ServerMembersController(_context).Create(serverMember2);

            return RedirectToAction(nameof(Index));
            //}
            //return View(server);
        }

        // GET: Servers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Servers == null)
            {
                return NotFound();
            }

            var server = await _context.Servers.FindAsync(id);
            if (server == null)
            {
                return NotFound();
            }
            return View(server);
        }

        // POST: Servers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServerName")] Server server)
        {
            if (id != server.Id)
            {
                return NotFound();
            }

            // ModelState is invalid since not all values are binded (or do different type of validation instead)
            // TODO: Fix later
            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(server);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServerExists(server.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            //return View(server);
        }

        // GET: Servers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Servers == null)
            {
                return NotFound();
            }

            var server = await _context.Servers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (server == null)
            {
                return NotFound();
            }

            return View(server);
        }

        // POST: Servers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Servers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Servers'  is null.");
            }
            var server = await _context.Servers.FindAsync(id);
            if (server != null)
            {
                _context.Servers.Remove(server);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServerExists(int id)
        {
            return (_context.Servers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
