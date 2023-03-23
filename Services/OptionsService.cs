using AdvancedProjectMVC.Data;
using AdvancedProjectMVC.Models;
using Microsoft.CodeAnalysis.Completion;

namespace AdvancedProjectMVC.Services
{
    public class OptionsService
    {
        private readonly ApplicationDbContext _context;

        public OptionsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Course> GetCourses()
        {
            List<Course> courses = new List<Course>();
            foreach(Course c in _context.Courses.ToList()) 
            {
                courses.Add(c);
            }
            return courses;
        }
    }
}
