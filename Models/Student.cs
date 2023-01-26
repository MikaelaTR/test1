namespace AdvancedProjectMVC.Models
{
    public class Student : ApplicationUser
    {
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
