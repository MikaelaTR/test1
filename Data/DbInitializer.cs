using AdvancedProjectMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdvancedProjectMVC.Data
{
    
    public class DbInitializer
    {
        public readonly IPasswordHasher<IdentityUser> _passwordHasher;
        public static void Initialize(ApplicationDbContext context)
        {
            
            // Look for any Admins.
            if (!context.Administrators.Any())
            {
                var admin = new Administrator()
                { 
                    AdminNumber = 1,
                    FirstName = "Alex", 
                    LastName = "B", 
                    Email = "alexb@school.ca", 
                    DOB = DateTime.Parse("1989-01-01"), 
                    DateRegistered = DateTime.Parse("2001-01-01"), 
                };
                context.Administrators.Add(admin);

                PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
                var hashedPassword = passwordHasher.HashPassword(admin, "Test123$");
                admin.PasswordHash = hashedPassword;
                
                context.SaveChanges();
            }

            if (!context.Instructors.Any())
            {
                var instructors = new Instructor[]
                {
                    new Instructor{InstructorNumber=1,FirstName="Ian",LastName="Moser",Email="ianm@school.ca",DOB=DateTime.Parse("1965-03-03"),DateRegistered=DateTime.Parse("1998-05-23")},
                };
                foreach (Instructor i in instructors)
                {
                    context.Instructors.Add(i);
                }
                context.SaveChanges();
            }
            
            if (!context.Students.Any())
            {
                var students = new Student[]
                {
                    new Student{StudentNumber=1,FirstName="Carson",LastName="Alexander",Email="calexander@school.ca",DOB=DateTime.Parse("1989-03-01"),DateRegistered=DateTime.Parse("2005-09-01")},
                    new Student{FirstName="Meredith",LastName="Alonso",Email="malonso@school.ca",DOB=DateTime.Parse("1991-03-01"),DateRegistered=DateTime.Parse("2002-09-01")},
                    new Student{FirstName="Arturo",LastName="Anand",Email="aarturo@school.ca",DOB=DateTime.Parse("2000-03-01"),DateRegistered=DateTime.Parse("2003-09-01")},
                    new Student{FirstName="Gytis",LastName="Barzdukas",Email="gbarzdukas@school.ca",DOB=DateTime.Parse("1999-03-01"),DateRegistered=DateTime.Parse("2002-09-01")},
                    new Student{FirstName="Yan",LastName="Li",Email="yli@school.ca",DOB=DateTime.Parse("2001-03-01"),DateRegistered=DateTime.Parse("2002-09-01")},
                    new Student{FirstName="Peggy",LastName="Justice",Email="pjustice@school.ca",DOB=DateTime.Parse("2002-03-01"),DateRegistered=DateTime.Parse("2001-09-01")},
                    new Student{FirstName="Laura",LastName="Norman",Email="lnorman@school.ca",DOB=DateTime.Parse("1994-03-01"),DateRegistered=DateTime.Parse("2003-09-01")},
                    new Student{FirstName="Nino",LastName="Olivetto",Email="nolivetto@school.ca",DOB=DateTime.Parse("1995-03-01"),DateRegistered=DateTime.Parse("2005-09-01")}
                };
                foreach (Student s in students)
                {
                    context.Students.Add(s);
                }
                context.SaveChanges();
            }
            
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
            

            var enrollments = new Enrollment[]
            {
/*            new Enrollment{StudentID=,CourseID=1,Grade=88},
            new Enrollment{StudentID=1,CourseID=2,Grade=54},
            new Enrollment{StudentID=1,CourseID=3,Grade=78},
            new Enrollment{StudentID=2,CourseID=1,Grade=98},
            new Enrollment{StudentID=2,CourseID=4,Grade=65},
            new Enrollment{StudentID=2,CourseID=6,Grade=72},
            new Enrollment{StudentID=3,CourseID=1},
            new Enrollment{StudentID=4,CourseID=7},
            new Enrollment{StudentID=4,CourseID=7,Grade=63},
            new Enrollment{StudentID=5,CourseID=5,Grade=55},
            new Enrollment{StudentID=6,CourseID=3},
            new Enrollment{StudentID=7,CourseID=3,Grade=2},*/
            };
            foreach (Enrollment e in enrollments)
            {
                context.Enrollments.Add(e);
            }
            context.SaveChanges();
        }
    }
}
