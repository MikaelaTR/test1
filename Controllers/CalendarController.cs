using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvancedProjectMVC.Data;
using AdvancedProjectMVC.Models;
using Microsoft.CodeAnalysis.Differencing;


namespace AdvancedProjectMVC.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalendarController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: CalendarController
        public async Task<IActionResult> Index()
        {
            return _context.Course != null ?
                          View(await _context.CalendarEvent.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.CalendarEvent'  is null.");
        }

        // GET: CalendarController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: CalendarController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CalendarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, UserID, Title, DateStart, DateEnd")] CalendarEvent calEvent)
        {
            if (ModelState.IsValid) {
                calEvent.UserID = User.Identity?.Name;
                _context.Add(calEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(calEvent);
        }

        // GET: CalendarController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CalendarEvent == null)
            {
                return NotFound();
            }

            // TODO: Make sure this is only available if userID is same as calendarEvent's userID
            var calEvent = await _context.CalendarEvent.FindAsync(id);
            if (calEvent == null)
            {
                return NotFound();
            }

            return View("Edit", calEvent);
        }

        // POST: CalendarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID, UserID, Title, DateStart, DateEnd")] CalendarEvent calEvent)
        {
            // Check if the calendar event ID is the same as before the redirect
            if (id != calEvent.ID)
            {
                return NotFound();
            }

            // Get calendar event by id
            var calEventCheck = await _context.CalendarEvent.FindAsync(id);

            // Check if the UserID from the form is the same as the one in the database
            // TODO: Make it more obvious to users why their edit save failed(?)
            if (calEvent.UserID.Equals(calEventCheck.UserID) && calEventCheck.UserID.Equals(User.Identity?.Name))
            {
                // Detach the variable from the context
                _context.Entry(calEventCheck).State = EntityState.Detached;

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(calEvent);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CalendarEventExists(calEvent.ID))
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
            }

            return View(calEvent);
        }

        // GET: CalendarController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Enrollment == null)
            {
                return NotFound();
            }

            var calEvent = await _context.CalendarEvent
                .FirstOrDefaultAsync(m => m.ID == id);

            if (calEvent == null)
            {
                return NotFound();
            }

            return View(calEvent);
        }

        // POST: CalendarController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private bool CalendarEventExists(int id)
        {
            return _context.CalendarEvent.Any(e => e.ID == id);
        }
    }
}
