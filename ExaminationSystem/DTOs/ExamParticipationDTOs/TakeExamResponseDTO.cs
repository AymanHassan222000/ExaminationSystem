using ExaminationSystem.DTOs.QuestionDTOs;

namespace ExaminationSystem.DTOs.ExamParticipationDTOs;

public class TakeExamResponseDTO
{
    public int ExamAttempitID { get; set; }
    public List<ExamQuestionsDTO> Questions { get; set; } = null!;
}
