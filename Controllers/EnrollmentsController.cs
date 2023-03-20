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
    [Authorize(Roles = "Admin,Student")]
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var user = User.Identity?.Name;

            // If not null
            if (user != null)
            {
                // Return only the list of events that the user has access to
                return _context.Enrollments != null ?
                              View(await _context.Enrollments.Where(m => m.ApplicationUserId == user).ToListAsync()) :
                              Problem("Entity set 'ApplicationDbContext.Enrollments'  is null.");
            }

            // Go to login page otherwise
            return View("Login");
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }

            var Enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Enrollment == null)
            {
                return NotFound();
            }

            return View(Enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            //Add dropdown list.
            List<SelectListItem> courses = new List<SelectListItem>();
            foreach(var course in _context.Courses.ToList())
            {
                courses.Add(new SelectListItem { Text = course.Title, Value = course.ID.ToString() });
            }
            ViewBag.Courses = courses;
            
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Grade")] Enrollment Enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }

            var Enrollment = await _context.Enrollments.FindAsync(id);
            if (Enrollment == null)
            {
                return NotFound();
            }
            return View(Enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EnrollmentCode,Title,Description,Location,Credits")] Enrollment Enrollment)
        {
            if (id != Enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(Enrollment.Id))
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
            return View(Enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }

            var Enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Enrollment == null)
            {
                return NotFound();
            }

            return View(Enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Enrollments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Enrollments'  is null.");
            }
            var Enrollment = await _context.Enrollments.FindAsync(id);
            if (Enrollment != null)
            {
                _context.Enrollments.Remove(Enrollment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return (_context.Enrollments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
