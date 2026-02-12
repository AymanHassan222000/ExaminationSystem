using ExaminationSystem.DTOs.QuestionDTOs;

namespace ExaminationSystem.DTOs.StudentDTOs;

public class TakeExamResponseDTO
{
    public int ExamAttempitID { get; set; }
    public List<ExamQuestionsDTO> Questions { get; set; } = null!;
}
