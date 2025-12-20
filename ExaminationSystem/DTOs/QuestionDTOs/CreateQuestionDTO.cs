using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.DTOs.QuestionDTOs;

public class CreateQuestionDTO
{
    public string QuestionText { get; set; } = null!;
    public QuestionLevel Level { get; set; }

    public int InstructorID { get; set; }

}
