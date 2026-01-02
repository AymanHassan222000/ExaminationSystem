namespace ExaminationSystem.ViewModels.InstructorViewModels;

public class AssignQuestionsToExamViewModel
{
    public int ExamID { get; set; }
    public List<int> QuestionIDs { get; set; } = new();

}
