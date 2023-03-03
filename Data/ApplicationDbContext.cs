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
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<TeachingAssignment> TeachingAssignments { get; set; }
        public DbSet<SchoolProgram> SchoolPrograms { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; } = default!;
        public DbSet<Server> Servers { get; set; }
        public DbSet<Channel> Channels { get; set; }
    }


}