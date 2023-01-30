namespace AdvancedProjectMVC.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool Submitted { get; set; }
        public DateTime? SubmittedDateTime { get; set; }
        public string Feedback { get; set; }
        public double Grade { get; set; }

        public Course Course { get; set; }
    }
}
