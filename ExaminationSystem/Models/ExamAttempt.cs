using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models;

public class ExamAttempt : BaseModel
{
    public int ExamID { get; set; }
    public int StudentID { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public bool IsSubmitted { get; set; }

    [ForeignKey("ExamID")]
    public Exam Exam { get; set; }

    [ForeignKey("StudentID")]
    public Student Student { get; set; }
}
