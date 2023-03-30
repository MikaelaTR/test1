using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvancedProjectMVC.Data;
using AdvancedProjectMVC.Models;

namespace AdvancedProjectMVC.Controllers
{
    public class ServerMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServerMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServerMembers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ServerMembers.Include(s => s.ApplicationUser).Include(s => s.Server);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ServerMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ServerMembers == null)
            {
                return NotFound();
            }

            var serverMember = await _context.ServerMembers
                .Include(s => s.ApplicationUser)
                .Include(s => s.Server)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serverMember == null)
            {
                return NotFound();
            }

            return View(serverMember);
        }

        // GET: ServerMembers/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ServerId"] = new SelectList(_context.Servers, "Id", "Id");
            return View();
        }

        // POST: ServerMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServerId,ApplicationUserId")] ServerMember serverMember)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == serverMember.ApplicationUserId);
                var server = await _context.Servers.FirstOrDefaultAsync(s => s.Id == serverMember.ServerId);
                serverMember.ApplicationUser = user;
                serverMember.Server = server;
                _context.Add(serverMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", serverMember.ApplicationUserId);
            ViewData["ServerId"] = new SelectList(_context.Servers, "Id", "Id", serverMember.ServerId);
            return View(serverMember);
        }

        // GET: ServerMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ServerMembers == null)
            {
                return NotFound();
            }

            var serverMember = await _context.ServerMembers.FindAsync(id);
            if (serverMember == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", serverMember.ApplicationUserId);
            ViewData["ServerId"] = new SelectList(_context.Servers, "Id", "Id", serverMember.ServerId);
            return View(serverMember);
        }

        // POST: ServerMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServerId,ApplicationUserId")] ServerMember serverMember)
        {
            if (id != serverMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serverMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServerMemberExists(serverMember.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", serverMember.ApplicationUserId);
            ViewData["ServerId"] = new SelectList(_context.Servers, "Id", "Id", serverMember.ServerId);
            return View(serverMember);
        }

        // GET: ServerMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ServerMembers == null)
            {
                return NotFound();
            }

            var serverMember = await _context.ServerMembers
                .Include(s => s.ApplicationUser)
                .Include(s => s.Server)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serverMember == null)
            {
                return NotFound();
            }

            return View(serverMember);
        }

        // POST: ServerMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ServerMembers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ServerMember'  is null.");
            }
            var serverMember = await _context.ServerMembers.FindAsync(id);
            if (serverMember != null)
            {
                _context.ServerMembers.Remove(serverMember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServerMemberExists(int id)
        {
            return (_context.ServerMembers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> CreateDM(String member1, String member2)
        {
            Server dm = new Server
            {
                ServerName = member1 + " + " + member2,
            };
            await new ServersController(_context).Create(dm);

            return RedirectToAction("Index", "Servers");
        }
    }
}
