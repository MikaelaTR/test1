using System;
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
    public class ServersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Servers
        public async Task<IActionResult> Index()
        {
            // return View();
            return _context.Servers != null ?
                        View(await _context.Servers.ToListAsync()) :
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
