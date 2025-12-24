using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models;

public class ExamAttemptAnswer : BaseModel
{
    public int ExamAtteptID { get; set; }
    public int QuestionID { get; set; }
    public int ChoiceID { get; set; }

    public bool IsCorrect { get; set; }


    [ForeignKey("ExamAtteptID")]
    public ExamAttempt ExamAttempt { get; set; }

    [ForeignKey("QuestionID")]
    public Question Question { get; set; }

    [ForeignKey("ChoiceID")]
    public Choice Choice { get; set; }
}
