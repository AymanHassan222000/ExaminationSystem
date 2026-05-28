namespace ExaminationSystem.API.ViewModels.ChoiceViewModels;

public record GetChoicesInfoViewModel(
    int ID,
    string ChoiceText,
    bool IsCorrect
);


