namespace ExaminationSystem.Models;

public class Instructor : User
{
    public ICollection<Course> Courses = new HashSet<Course>();
    public ICollection<Question> Questions = new HashSet<Question>();
}
