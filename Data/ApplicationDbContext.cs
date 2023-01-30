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
        public DbSet<Course> Course { get; set; } = default!;
        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
        public DbSet<TeachingAssignment> TeachingAssignment { get; set; }
        public DbSet<SchoolProgram> SchoolProgram { get; set; }
    }
}