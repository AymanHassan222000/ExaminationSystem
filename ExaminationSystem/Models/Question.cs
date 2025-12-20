using ExaminationSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models;

public class Question : BaseModel
{
    [Required]
    public string QuestionText { get; set; } = null!;
    public QuestionLevel Level { get; set; }

    public int InstructorID { get; set; }

    [ForeignKey("InstructorID")]
    public Instructor Instructor { get; set; }

    public ICollection<ExamQuestion> ExamQuestions = new List<ExamQuestion>();
    public ICollection<Choice> Choices = new List<Choice>();
}
