namespace ExaminationSystem.ViewModels.ExamViewModels;

public record UpdateExamViewModel(
    int ID,
    string? Title,
    ExamTypes? Type = null,
    int? DurationInMinutes = null,
    DateTime? Date = null,
    TimeOnly? StartTime = null,
    TimeOnly? EndTime = null
);
