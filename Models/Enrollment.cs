using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvancedProjectMVC.Models
{
    public class Enrollment
    {
        public int ID { get; set; }
        public int CourseID { get; set; }
        [ForeignKey("Student")]
        public string StudentID { get; set; }

        [DisplayFormat(NullDisplayText = "No grade")]
        public double? Grade { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
