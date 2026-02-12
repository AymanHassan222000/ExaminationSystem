using ExaminationSystem.ViewModels.InstructorViewModels;

namespace ExaminationSystem.ViewModels.QuestionViewModel;

public class QuestionDetailsViewModel
{
    public int QuestionID { get; set; }
    public string QuestionText { get; set; } = null!;
    public QuestionLevel Level { get; set; }

    public GetInstructorInfoViewModel instructorInfo { get; set; } = null!;

}
