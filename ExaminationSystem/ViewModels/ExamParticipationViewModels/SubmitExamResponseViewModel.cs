namespace ExaminationSystem.ViewModels.ExamParticipationViewModels;

public class SubmitExamResponseViewModel
{
    public int ExamAttemptID { get; set; }
    public bool IsSubmitted { get; set; }
    public DateTime SubmittedAt { get; set; }

}
