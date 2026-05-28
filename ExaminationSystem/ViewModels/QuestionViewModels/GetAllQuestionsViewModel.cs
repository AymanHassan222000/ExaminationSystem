namespace ExaminationSystem.API.ViewModels.QuestionViewModels;

public record GetAllQuestionsViewModel(
    int ID,
    string Head,
    int Grade,
    QuestionLevel Level,
    int CourseID
);
