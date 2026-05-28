using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DAL.Models;

public class Course : BaseModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;    
    public int Hours { get; set; }
    public int InstructorID { get; set; }

    [ForeignKey(nameof(InstructorID))]
    public Instructor Instructor { get; set; }

    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

    public ICollection<Exam> Exams { get; set; } = new HashSet<Exam>();

    [InverseProperty(nameof(PreRequesit.MainCourse))]
    public ICollection<PreRequesit> MainCourses { get; set; } = new HashSet<PreRequesit>();

    [InverseProperty(nameof(PreRequesit.PreRequisiteCourse))]
    public ICollection<PreRequesit> PreRequesits { get; set; } = new HashSet<PreRequesit>();
}
