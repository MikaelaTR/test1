namespace AdvancedProjectMVC.Models
{
    public class Student : ApplicationUser
    {
        public int StudentID { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
