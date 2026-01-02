namespace ExaminationSystem.ViewModels.ResultEvaluationViewModels;

public class EvaluateExamResponseViewModel
{
    public int ExamAttemptID { get; set; }

    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }

    public double Percentage { get; set; }
    public bool IsPassed { get; set; }

}
