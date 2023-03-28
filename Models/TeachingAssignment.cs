namespace AdvancedProjectMVC.Models
{
    public class TeachingAssignment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string ApplicationUserId { get; set; }

        public Course Course { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
