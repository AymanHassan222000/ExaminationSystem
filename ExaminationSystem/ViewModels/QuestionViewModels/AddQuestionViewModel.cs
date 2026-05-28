using ExaminationSystem.API.ViewModels.ChoiceViewModels;

namespace ExaminationSystem.ViewModels.QuestionViewModel;

public record AddQuestionViewModel(
    string Head,
    int Grade,
    QuestionLevel Level,
    int CourseID,
    IList<AddChoiceViewModel> Choices
);

