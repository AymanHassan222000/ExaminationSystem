using ExaminationSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DAL.Models;

public class Exam : BaseModel
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = null!;
    public ExamTypes Type { get; set; }
    public int DurationInMinutes { get; set; }
    public DateTime Date { get; set; } 
    public TimeOnly StartTime { get; set; } 
    public TimeOnly EndTime { get; set; } 

    public int CourseID { get; set; }

    [ForeignKey(nameof(CourseID))]
    public Course Course { get; set; }

    public ICollection<ExamQuestion> ExamQuestions { get; set; } = new HashSet<ExamQuestion>();
    public ICollection<ExamAttempt> ExamAttempts { get; set; } = new HashSet<ExamAttempt>();

}
