using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models;

public class Choice : BaseModel
{
    [Required]
    public string ChoiceText { get; set; } = null!;
    public bool IsCorrect { get; set; } = false;
    public int QuestionID { get; set; }

    [ForeignKey("QuestionID")]
    public Question Question { get; set; }
}
