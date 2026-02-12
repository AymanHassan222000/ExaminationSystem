using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models;

public class Student : BaseModel
{
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

    public int UserID { get; set; }

    [ForeignKey(nameof(UserID))]
    public User User { get; set; }
}
