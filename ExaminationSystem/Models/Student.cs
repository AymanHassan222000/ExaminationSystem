namespace ExaminationSystem.Models;

public class Student : User
{
    public ICollection<StudentCourse> StudentCourses = new List<StudentCourse>();
}
