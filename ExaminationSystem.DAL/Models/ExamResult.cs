using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DAL.Models;

public class ExamResult : BaseModel
{
    public int ExamDegree { get; set; }
    public int StudentScore { get; set; }
    public double Percentage { get;set; }
    public bool IsPassed { get; set; }
    public int ExamAttemptID { get; set; }

    [ForeignKey("ExamAttemptID")]
    public ExamAttempt ExamAttempt { get; set; }
}
