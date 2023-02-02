using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AdvancedProjectMVC.Models;

namespace AdvancedProjectMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<TeachingAssignment> TeachingAssignments { get; set; }
        public DbSet<SchoolProgram> SchoolPrograms { get; set; }
        public DbSet<AdvancedProjectMVC.Models.ChatMessage> ChatMessage { get; set; } = default!;
    }


}