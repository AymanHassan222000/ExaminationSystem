namespace ExaminationSystem.DAL.Models;

public class Instructor : BaseModel
{
    public ICollection<Course> Courses { get; set; } = new HashSet<Course>();
    public int UserID { get; set; }
    public User User { get; set; }
}
 