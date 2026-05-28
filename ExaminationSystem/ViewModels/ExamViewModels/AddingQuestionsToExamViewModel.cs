namespace ExaminationSystem.API.ViewModels.ExamViewModels;

public record AddingQuestionsToExamViewModel(
    int ExamID,
    IList<int> QuestionIDs
);
