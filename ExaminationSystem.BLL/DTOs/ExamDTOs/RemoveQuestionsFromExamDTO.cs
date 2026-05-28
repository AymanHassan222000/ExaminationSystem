namespace ExaminationSystem.DTOs.ExamDTOs;

public record RemoveQuestionsFromExamDTO(
    int ExamID,
    IList<int> QuestionIDs 
);
