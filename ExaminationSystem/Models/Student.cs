namespace ExaminationSystem.Models;

public class Student : User
{
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}
