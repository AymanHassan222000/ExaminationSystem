using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models;

public class ExamResult : BaseModel
{
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }

    public double Percentage { get;set; }
    public bool IsPassed { get; set; }

    public int ExamAttemptID { get; set; }

    [ForeignKey("ExamAttemptID")]
    public ExamAttempt ExamAttempt { get; set; }
}
