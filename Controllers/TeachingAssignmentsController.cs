using AdvancedProjectMVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProjectMVC.Controllers
{
    [Authorize]
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
            return _context.TeachingAssignments != null
                ? View(await _context.TeachingAssignments.ToListAsync())
                : Problem("Entity set 'ApplicationDbContext.Enrollments'  is null.");
        }

    }
}