namespace ExaminationSystem.API.ViewModels.QuestionViewModels;

public record UpdateQuestionChoiceViewModel(
    int QuestionID,
    int ChoiceID,
    string? Text = null
);

