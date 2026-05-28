using ExaminationSystem.ViewModels.QuestionViewModel;

namespace ExaminationSystem.API.ViewModels.ExamViewModels;

public record GetExamByIdViewModel(
    string Title,
    ExamTypes Type,
    int DurationInMinutes,
    TimeOnly StartTime,
    TimeOnly EndTime,
    IList<ExamQuestionsViewModel> Questions
);
