using ExaminationSystem.DTOs.IntructorDTOs;

namespace ExaminationSystem.DTOs.QuestionDTOs;

public sealed class GetAllQuestionsDTO
{
    public int ID { get; set; }
    public string Head { get; set; } = null!;
    public int Grade { get; set; }
    public QuestionLevel Level { get; set; }
    public int CourseID { get; set; }
}
