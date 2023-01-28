namespace AdvancedProjectMVC.Models
{
    public class Instructor : ApplicationUser
    {
        public int InstructorNumber { get; set; }
        public string? Office { get; set; }

        public ICollection<TeachingAssignment>? TeachingAssignments { get; set;}

    }
}
