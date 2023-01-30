namespace AdvancedProjectMVC.Models
{
    public class Student : ApplicationUser
    {
        public int StudentNumber { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
