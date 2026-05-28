using ExaminationSystem.API.ViewModels.ChoiceViewModels;
using ExaminationSystem.ViewModels.CourseViewModels;

namespace ExaminationSystem.API.ViewModels.QuestionViewModels;

public record GetQuestionByIdResponseViewModel(
    int ID,
    string Head,
    int Grade,
    QuestionLevel Level,
    GetCourseInfoViewModel Course,
    IEnumerable<GetChoicesInfoViewModel> Choices
);

