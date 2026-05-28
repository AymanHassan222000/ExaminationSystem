using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DAL.Models;

public class Student : BaseModel
{
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    public ICollection<ExamAttempt> ExamAttempts { get; set; } = new HashSet<ExamAttempt>();

    public int UserID { get; set; }

    [ForeignKey(nameof(UserID))]
    public User User { get; set; }
}
