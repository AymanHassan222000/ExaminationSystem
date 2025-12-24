
namespace ExaminationSystem.ViewModels.ExamParticipationViewModels;

public class SubmitExamRequestViewModel
{
    public int ExamAttempitID { get; set; }
    public List<SubmitExamAnswerViewModel> Answers { get; set; }
}