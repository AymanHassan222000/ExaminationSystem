using ExaminationSystem.DTOs.StudentDTO;

namespace ExaminationSystem.DTOs.ResultEvaluationDTOs;

public class ExamResultSummaryDTO
{

    public GetStudentInfoDTO StudentInfo { get; set; }

    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public double Persentage { get; set; }
    public bool IsPassed { get; set; }
}
