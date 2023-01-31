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
        public async Task<IActionResult> Create([Bind("ID, UserID, Title, DateStart, DateEnd")] CalendarEvent CalEvent)
        {
            if (ModelState.IsValid) {
                _context.Add(CalEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(CalEvent);
            /*
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
            */
        }

        // GET: CalendarController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: CalendarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: CalendarController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
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
    }
}
