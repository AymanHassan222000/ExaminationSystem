namespace ExaminationSystem.API.ViewModels.ExamViewModels;

public record RemoveQuestionsFromExamViewModel(
    int ExamID,
    IList<int> QuestionIDs
);
