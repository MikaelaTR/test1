using System.ComponentModel.DataAnnotations;

namespace AdvancedProjectMVC.Models
{
    public class Course
    {
        public int Id { get; set; }
        [RegularExpression(@"^[a-zA-Z]{4}-?[0-9]{4}(-[a-zA-Z]{2}[a-zA-z]?(/[a-zA-z]{2}[a-zA-Z]?)?)?$")]
        public string CourseCode { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public int Credits { get; set; }

        public ICollection<Enrollment>? Enrollments { get; set; }
        public ICollection<SchoolProgram>? Programs { get; set; }
        public ICollection<Assignment>? Assignments { get; set; }
    }
}
