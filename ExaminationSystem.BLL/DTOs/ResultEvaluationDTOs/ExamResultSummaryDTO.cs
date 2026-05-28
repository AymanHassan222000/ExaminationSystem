using ExaminationSystem.DTOs.StudentDTO;

namespace ExaminationSystem.DTOs.ResultEvaluationDTOs;

public sealed class ExamResultSummaryDTO
{
    public int StudentID { get; set; }
    public string StudentName { get; set; }
    public int ExamDegree { get; set; }
    public int StudentScore { get; set; }
    public double Persentage { get; set; }
    public bool IsPassed { get; set; }
    public DateTime ExamDate { get; set; }
}
