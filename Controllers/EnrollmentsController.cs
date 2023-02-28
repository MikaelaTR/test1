using AdvancedProjectMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProjectMVC.Controllers
{
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
            return _context.Enrollments != null
                ? View(await _context.Enrollments.ToListAsync())
                : Problem("Entity set 'ApplicationDbContext.Enrollments'  is null.");
        }
    }
}