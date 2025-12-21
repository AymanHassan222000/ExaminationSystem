namespace ExaminationSystem.Models;

public class Instructor : User
{
    public ICollection<Course> Courses { get; set; } = new HashSet<Course>();
    public ICollection<Question> Questions { get; set; } = new HashSet<Question>();
}
