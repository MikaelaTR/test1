namespace AdvancedProjectMVC.Models
{
    public class SchoolProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LengthInYears { get; set; }
        public int Semesters { get; set; }
        public double Tuition { get; set; }
        public bool Coop { get; set; }

        public ICollection<Course> Courses { get; set; }
        public ICollection<Student> Students { get; set; }
        
    }
}
