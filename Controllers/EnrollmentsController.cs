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
using NuGet.ProjectModel;
using System.ComponentModel.DataAnnotations;

namespace AdvancedProjectMVC.Controllers
{
    [Authorize]
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EnrollmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var username = User.Identity?.Name;
            var user = await _userManager.GetUserAsync(HttpContext.User);

            // If not null
            if (username != null)
            {
                // Return only the list of events that the user has access to
                return _context.Enrollments != null ?
                              View(await _context.Enrollments.Where(m => m.ApplicationUser == user).Include(m => m.Course).ToListAsync()) :
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
            /*            //Add dropdown list.
                         List<SelectListItem> courses = new List<SelectListItem>();
                         foreach (var course in _context.Courses.ToList())
                         {
                             courses.Add(new SelectListItem { Text = course.Title, Value = course.Id.ToString() });
                         }
            *//*           List<Course> courses = _context.Courses.ToList();*//*
                         ViewData["Courses"] = new SelectList(courses);

                        *//*            var courses = _context.Courses.ToList();
                                    ViewBag.Courses = courses;*/

            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Title");

            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,AppliationUserId,Grade,Course,ApplicationUser")] Enrollment Enrollment)
        {
            // Create new validation context for Enrollment
            var validationContext = new ValidationContext(Enrollment);

            var enrollment = Enrollment;
            var user = await _userManager.GetUserAsync(User);
            var course = await _context.Courses.FindAsync(Enrollment.CourseId);

            // If course is not found from the list
            if (course == null) {
                return NotFound();
            }

            Enrollment.Course = course;
            Enrollment.ApplicationUser = user;
            Enrollment.ApplicationUserId = user.Id;

            // Revalidate the model (in case something funky happens and user/user.id is null)
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(Enrollment, validationContext, results);

            if (isValid)
            {
                _context.Add(Enrollment);
                await _context.SaveChangesAsync();

                //Find the Course's Server. This assumes no two of the same server names in the db.
                string serverName = course.Title;
                Server server = _context.Servers.Where(s => s.ServerName == serverName).ToList().First();

                ServerMember serverMember = new ServerMember();
                serverMember.ApplicationUserId = user.Id;
                serverMember.ServerId = server.Id;
                await new ServerMembersController(_context).Create(serverMember);

                return RedirectToAction(nameof(Index));
            }

            return View(Enrollment);
        }

        // GET: Enrollments/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Enrollment Enrollment)
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
