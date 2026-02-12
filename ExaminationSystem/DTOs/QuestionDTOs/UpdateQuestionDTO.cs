namespace ExaminationSystem.DTOs.QuestionDTOs;

public class UpdateQuestionDTO
{
    public string QuestionText { get; set; } = null!;
    public QuestionLevel Level { get; set; }

}
