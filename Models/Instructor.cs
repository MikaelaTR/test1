namespace AdvancedProjectMVC.Models
{
    public class Instructor : ApplicationUser
    {
        public int InstructorID { get; set; }
        public double Salary { get; set; }
        public string? Office { get; set; }

        public ICollection<TeachingAssignment>? TeachingAssignments { get; set;}

    }
}
