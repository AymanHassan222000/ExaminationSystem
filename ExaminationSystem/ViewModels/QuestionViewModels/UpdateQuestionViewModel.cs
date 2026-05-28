namespace ExaminationSystem.ViewModels.QuestionViewModel;

public record UpdateQuestionViewModel(
    int ID,
    string? Head = null,
    int? Grade = null,
    QuestionLevel? Level = null
);

