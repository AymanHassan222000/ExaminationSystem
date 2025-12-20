using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.ViewModels.QuestionViewModel;

public class CreateQuestionViewModel
{
    public string QuestionText { get; set; } = null!;
    public QuestionLevel Level { get; set; }

    public int InstructorID { get; set; }

}
