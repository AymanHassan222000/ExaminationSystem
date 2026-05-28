namespace ExaminationSystem.DTOs.StudentDTOs;

public record SubmitExamRequestDTO(
    int ExamID,
    IList<SubmitExamAnswerDTO> Answers
);
