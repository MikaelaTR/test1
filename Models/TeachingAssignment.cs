namespace AdvancedProjectMVC.Models
{
    public class TeachingAssignment
    {
        public int ID { get; set; }
        public int CourseID { get; set; }

        public Course Course { get; set; }
        public Instructor Instructor { get; set; }
    }
}
