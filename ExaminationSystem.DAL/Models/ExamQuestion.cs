using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.DAL.Models;

public class ExamQuestion : BaseModel
{
    public int ExamID { get; set; }
    public int QuestionID { get; set; }

    [ForeignKey(nameof(ExamID))]
    public Exam Exam { get; set; }

    [ForeignKey(nameof(QuestionID))]
    public Question Question { get; set; }
}
