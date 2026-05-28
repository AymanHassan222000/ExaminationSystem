using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DAL.Models;

public class ExamAttempt : BaseModel
{
    public int ExamID { get; set; }
    public int StudentID { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public bool IsSubmitted { get; set; } = false;
    public bool IsTakenExam { get; set; } = false;

    [ForeignKey(nameof(ExamID))]
    public Exam Exam { get; set; }

    [ForeignKey(nameof(StudentID))]
    public Student Student { get; set; }

    public ICollection<ExamAttemptAnswer> Answers { get; set; } = new List<ExamAttemptAnswer>();

    public ExamResult ExamResult { get; set; }
}
