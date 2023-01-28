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
    public class TeachingAssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeachingAssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TeachingAssignments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TeachingAssignment.Include(t => t.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TeachingAssignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TeachingAssignment == null)
            {
                return NotFound();
            }

            var teachingAssignment = await _context.TeachingAssignment
                .Include(t => t.Course)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teachingAssignment == null)
            {
                return NotFound();
            }

            return View(teachingAssignment);
        }

        // GET: TeachingAssignments/Create
        public IActionResult Create()
        {
            ViewData["CourseID"] = new SelectList(_context.Course, "ID", "ID");
            return View();
        }

        // POST: TeachingAssignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CourseID")] TeachingAssignment teachingAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teachingAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseID"] = new SelectList(_context.Course, "ID", "ID", teachingAssignment.CourseID);
            return View(teachingAssignment);
        }

        // GET: TeachingAssignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TeachingAssignment == null)
            {
                return NotFound();
            }

            var teachingAssignment = await _context.TeachingAssignment.FindAsync(id);
            if (teachingAssignment == null)
            {
                return NotFound();
            }
            ViewData["CourseID"] = new SelectList(_context.Course, "ID", "ID", teachingAssignment.CourseID);
            return View(teachingAssignment);
        }

        // POST: TeachingAssignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CourseID")] TeachingAssignment teachingAssignment)
        {
            if (id != teachingAssignment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teachingAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeachingAssignmentExists(teachingAssignment.ID))
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
            ViewData["CourseID"] = new SelectList(_context.Course, "ID", "ID", teachingAssignment.CourseID);
            return View(teachingAssignment);
        }

        // GET: TeachingAssignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TeachingAssignment == null)
            {
                return NotFound();
            }

            var teachingAssignment = await _context.TeachingAssignment
                .Include(t => t.Course)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teachingAssignment == null)
            {
                return NotFound();
            }

            return View(teachingAssignment);
        }

        // POST: TeachingAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TeachingAssignment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TeachingAssignment'  is null.");
            }
            var teachingAssignment = await _context.TeachingAssignment.FindAsync(id);
            if (teachingAssignment != null)
            {
                _context.TeachingAssignment.Remove(teachingAssignment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeachingAssignmentExists(int id)
        {
          return (_context.TeachingAssignment?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
