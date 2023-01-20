namespace AdvancedProjectMVC.Models
{
    public class Enrollment
    {
        public int ID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public double? Grade { get; set; }

        public Course Course { get; set; }
    }
}
