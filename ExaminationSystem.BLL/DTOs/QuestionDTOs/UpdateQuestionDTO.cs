namespace ExaminationSystem.DTOs.QuestionDTOs;

public record UpdateQuestionDTO(
    int ID,
    string? Head = null,
    int? Grade = null,
    QuestionLevel? Level = null
);
