using ExaminationSystem.ViewModels.QuestionViewModel;

namespace ExaminationSystem.ViewModels.StudentViewModels;

public class TakeExamResponseViewModel
{
    public int ExamAttempitID { get; set; }
    public List<ExamQuestionsViewModel> Questions { get; set; } = null!;
}
