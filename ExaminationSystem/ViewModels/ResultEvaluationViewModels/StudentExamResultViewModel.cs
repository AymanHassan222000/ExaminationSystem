using ExaminationSystem.ViewModels.ExamViewModels;

namespace ExaminationSystem.ViewModels.ResultEvaluationViewModels;

public class StudentExamResultViewModel
{
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public double Percentage { get; set; }
    public bool IsPassed { get; set; }

    public GetExamInfoViewModel ExamInfo { get; set; }

}
