namespace AdvancedProjectMVC.Models
{
    public class Instructor : ApplicationUser
    {
        public string? Office { get; set; }

        public ICollection<TeachingAssignment>? TeachingAssignments { get; set;}

    }
}
