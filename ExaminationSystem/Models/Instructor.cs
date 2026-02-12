using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models;

public class Instructor : BaseModel
{
    public ICollection<Course> Courses { get; set; } = new HashSet<Course>();
    public ICollection<Question> Questions { get; set; } = new HashSet<Question>();

    public int UserID { get; set; }

    [ForeignKey(nameof(UserID))]
    public User User { get; set; }
}
