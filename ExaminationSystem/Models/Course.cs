using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ExaminationSystem.Models;

public class Course : BaseModel
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;    
    public int Hours { get; set; }
    public int InstructorID { get; set; }

    [ForeignKey("InstructorID")]
    public Instructor Instructor { get; set; }

    public ICollection<Exam> Exams { get; set; } = new List<Exam>();
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}
