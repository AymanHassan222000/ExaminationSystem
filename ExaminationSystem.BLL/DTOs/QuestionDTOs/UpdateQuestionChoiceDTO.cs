namespace ExaminationSystem.BLL.DTOs.QuestionDTOs;

public record UpdateQuestionChoiceDTO
(
    int QuestionID,
    int ChoiceID,
    string? Text = null
);
