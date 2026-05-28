using ExaminationSystem.API.ViewModels.StudentViewModels;

namespace ExaminationSystem.API.ViewModels.ResultEvaluationViewModels;

public record ExamResultSummaryViewModel(
    int StudentID,
    string StudentName,
    int ExamDegree,
    int StudentScore,
    double Persentage,
    bool IsPassed,
    DateTime ExamDate
);
