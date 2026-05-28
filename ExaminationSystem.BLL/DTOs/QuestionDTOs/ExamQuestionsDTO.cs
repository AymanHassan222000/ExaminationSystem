using ExaminationSystem.DTOs.ChoiceDTOs;

namespace ExaminationSystem.DTOs.QuestionDTOs;

public sealed class ExamQuestionsDTO
{
    public int QuestionID { get; set; }
    public string QuestionText { get; set; } = null!;
    public IList<GetExamChoicesInfoDTO> Choices { get; set; }
}
