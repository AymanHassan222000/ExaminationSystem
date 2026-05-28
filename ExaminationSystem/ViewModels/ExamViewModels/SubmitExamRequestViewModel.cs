namespace ExaminationSystem.API.ViewModels.ExamViewModels;

public record SubmitExamRequestViewModel(
    int ExamID,
    IList<SubmitExamAnswerViewModel> Answers
);
