using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models;

public class StudentAnswer : BaseModel
{
    public int QuestionID { get; set; }
    public int ChoiceID { get; set; }
    public int StudentID { get; set; }

    [ForeignKey("QuestionID")]
    public Question Question { get; set; }

    [ForeignKey("ChoiceID")]
    public Choice Choice { get; set; }

    [ForeignKey("StudentID")]
    public Student Student { get; set; }
}
