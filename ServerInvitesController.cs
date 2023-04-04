using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvancedProjectMVC.Data;
using AdvancedProjectMVC.Models;
using Microsoft.AspNetCore.Identity;
using AdvancedProjectMVC.Controllers;

namespace AdvancedProjectMVC
{
    public class ServerInvitesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServerInvitesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ServerInvites
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ServerInvites.Include(s => s.ApplicationUser).Include(s => s.Server);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ServerInvites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ServerInvites == null)
            {
                return NotFound();
            }

            var serverInvite = await _context.ServerInvites
                .Include(s => s.ApplicationUser)
                .Include(s => s.Server)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serverInvite == null)
            {
                return NotFound();
            }

            return View(serverInvite);
        }

        // GET: ServerInvites/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["serverId"] = new SelectList(_context.Servers, "Id", "ServerName");
            return View();
        }

        // POST: ServerInvites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,serverId,ApplicationUserId")] ServerInvite serverInvite)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                serverInvite.SenderUserName = user.UserName;
                serverInvite.DateSent= DateTime.Now;
                _context.Add(serverInvite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "UserName", serverInvite.ApplicationUserId);
            ViewData["serverId"] = new SelectList(_context.Servers, "Id", "ServerName", serverInvite.serverId);
            return View(serverInvite);
        }

        // GET: ServerInvites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ServerInvites == null)
            {
                return NotFound();
            }

            var serverInvite = await _context.ServerInvites.FindAsync(id);
            if (serverInvite == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", serverInvite.ApplicationUserId);
            ViewData["serverId"] = new SelectList(_context.Servers, "Id", "Id", serverInvite.serverId);
            return View(serverInvite);
        }

        // POST: ServerInvites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,serverId,ApplicationUserId,DateSent")] ServerInvite serverInvite)
        {
            if (id != serverInvite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serverInvite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServerInviteExists(serverInvite.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", serverInvite.ApplicationUserId);
            ViewData["serverId"] = new SelectList(_context.Servers, "Id", "Id", serverInvite.serverId);
            return View(serverInvite);
        }

        // GET: ServerInvites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ServerInvites == null)
            {
                return NotFound();
            }

            var serverInvite = await _context.ServerInvites
                .Include(s => s.ApplicationUser)
                .Include(s => s.Server)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serverInvite == null)
            {
                return NotFound();
            }

            return View(serverInvite);
        }

        // POST: ServerInvites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ServerInvites == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ServerInvites'  is null.");
            }
            var serverInvite = await _context.ServerInvites.FindAsync(id);
            if (serverInvite != null)
            {
                _context.ServerInvites.Remove(serverInvite);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServerInviteExists(int id)
        {
          return (_context.ServerInvites?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Join(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var server = await _context.Servers.FindAsync(id);

            //Add user as serverMember
            ServerMember serverMember = new ServerMember();
            serverMember.ApplicationUserId = user.Id;
            serverMember.ServerId = server.Id;
            await new ServerMembersController(_context).Create(serverMember);

            return RedirectToAction("Index", "Servers");
        }
    }
}
