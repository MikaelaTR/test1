using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvancedProjectMVC.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }

        [DisplayFormat(NullDisplayText = "No grade")]
        public double? Grade { get; set; }

        public Course Course { get; set; }
        [Required]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
