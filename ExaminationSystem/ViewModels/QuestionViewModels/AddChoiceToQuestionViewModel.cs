using ExaminationSystem.API.ViewModels.ChoiceViewModels;

namespace ExaminationSystem.API.ViewModels.QuestionViewModels;

public record AddChoiceToQuestionViewModel(int QuestionID, IList<AddChoiceViewModel> Choices);
