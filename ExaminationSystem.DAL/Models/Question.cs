using ExaminationSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.DAL.Models;

public sealed class Question : BaseModel
{
    [Required]
    public string Head { get; set; } = null!;
    public int Grade { get; set; }
    public QuestionLevel Level { get; set; }
    public int CourseID { get; set; }
    public Course Course { get; set; }

    public ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
    public ICollection<Choice> Choices { get; set; } = new List<Choice>();
}
