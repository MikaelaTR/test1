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

namespace AdvancedProjectMVC.Controllers
{
    [Authorize]
    public class ChatMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChatMessages
        public async Task<IActionResult> Index()
        {
            var messages = await _context.ChatMessages
                .Include(x => x.ApplicationUser)
                .Include(c => c.Channel)
                .ToListAsync();

              return _context.ChatMessages != null ? 
                          View(messages) :
                          Problem("Entity set 'ApplicationDbContext.ChatMessages'  is null.");
        }

        // GET: ChatMessages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ChatMessages == null)
            {
                return NotFound();
            }

            var chatMessage = await _context.ChatMessages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatMessage == null)
            {
                return NotFound();
            }

            return View(chatMessage);
        }

        // GET: ChatMessages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChatMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationUserId,Content,DatePosted")] ChatMessage chatMessage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chatMessage);
        }

        // GET: ChatMessages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ChatMessages == null)
            {
                return NotFound();
            }

            var chatMessage = await _context.ChatMessages.FindAsync(id);
            if (chatMessage == null)
            {
                return NotFound();
            }
            return View(chatMessage);
        }

        // POST: ChatMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationUserId,Content,DatePosted")] ChatMessage chatMessage)
        {
            if (id != chatMessage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatMessageExists(chatMessage.Id))
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
            return View(chatMessage);
        }

        // GET: ChatMessages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ChatMessages == null)
            {
                return NotFound();
            }

            var chatMessage = await _context.ChatMessages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatMessage == null)
            {
                return NotFound();
            }

            return View(chatMessage);
        }

        // POST: ChatMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ChatMessages == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ChatMessage'  is null.");
            }
            var chatMessage = await _context.ChatMessages.FindAsync(id);
            if (chatMessage != null)
            {
                _context.ChatMessages.Remove(chatMessage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatMessageExists(int id)
        {
          return (_context.ChatMessages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
