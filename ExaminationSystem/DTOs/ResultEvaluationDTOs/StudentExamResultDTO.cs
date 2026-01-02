using ExaminationSystem.DTOs.ExamDTOs;

namespace ExaminationSystem.DTOs.ResultEvaluationDTOs;

public class StudentExamResultDTO
{
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public double Percentage { get; set; }
    public bool IsPassed { get; set; }

    public GetExamInfoDTO ExamInfo { get; set; }
}
