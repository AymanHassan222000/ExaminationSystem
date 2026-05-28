using ExaminationSystem.ViewModels.ChoiceViewModel;

namespace ExaminationSystem.ViewModels.QuestionViewModel;

public class ExamQuestionsViewModel
{
    public int QuestionID { get; set; }
    public string QuestionText { get; set; } = null!;
    public List<GetExamChoicesInfoViewModel> Choices { get; set; } = null!;

}
