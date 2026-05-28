namespace ExaminationSystem.DTOs.ResultEvaluationDTOs;

public record EvaluateExamResponseDTO(
    int ExamDegree, 
    int StudentScore,
    double Percentage,
    bool IsPassed
);