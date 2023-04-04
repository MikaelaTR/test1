using AdvancedProjectMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdvancedProjectMVC.Data
{
    
    public class DbInitializer
    {
        public readonly IPasswordHasher<IdentityUser> _passwordHasher;

        public enum Roles
        {
            SuperAdmin,
            Admin,
            Instructor,
            Student
        }

        // Initialize DB with default models.
        public static void Initialize(ApplicationDbContext context)
        {         
            if (!context.Courses.Any())
            {
                var courses = new Course[]
                {
                    new Course{Title="Clinical Decision Support",CourseCode="COMP4111",Credits=1},
                    new Course{Title="Introduction to Data Science",CourseCode="COMP4112",Credits=1},
                    new Course{Title="Programming Language Processors",CourseCode="COMP4413",Credits=1},
                    new Course{Title="Algorithm Design and Analysis",CourseCode="COMP4433",Credits=4},
                    new Course{Title="Theory of Computing",CourseCode="COMP4451",Credits=4},
                    new Course{Title="Computer Graphics",CourseCode="COMP4471",Credits=3},
                    new Course{Title="Topics in Artificial Intelligence",CourseCode="COMP4475",Credits=4}
                };
                foreach (Course c in courses)
                {
                    context.Courses.Add(c);
                }
                context.SaveChanges();
            }

            //Add test server
            if (!context.Servers.Any())
            {
                var server = new Server { ServerName = "Test Server" };
                context.Servers.Add(server);

                var channel = new Channel { ChannelName = "General", Server = server };
                context.Channels.Add(channel);

                context.SaveChanges();
            }
        }



        // Seed DB with default roles.
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Instructor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Student.ToString()));
        }

        // Seed DB with default user with all roles.
        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User.
            var defaultUser = new ApplicationUser
            {
                UserName = "ab",
                Email = "ab@lakeheadu.ca",
                FirstName = "Alex",
                LastName = "Blom",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Test123$.");
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Instructor.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Student.ToString());
                }
            }
        }

        public static async Task SeedStudentsAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var student = new ApplicationUser
            {
                UserName = "rm",
                Email = "rm@school.ca",
                FirstName = "Ryan",
                LastName = "McGill",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            if (userManager.Users.All(u => u.Id != student.Id))
            {
                var user = await userManager.FindByEmailAsync(student.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(student, "Test123$.");
                    await userManager.AddToRoleAsync(student, Roles.Student.ToString());
                }
            }
        }

        internal static Task SeedServersAsync(ApplicationDbContext context)
        {
            throw new NotImplementedException();
        }
    }
}
