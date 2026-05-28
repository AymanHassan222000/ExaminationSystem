namespace ExaminationSystem.ViewModels.ResultEvaluationViewModels;

public record EvaluateExamResponseViewModel(
    int ExamDegree,
    int StudentScore,
    double Percentage,
    bool IsPassed
);
