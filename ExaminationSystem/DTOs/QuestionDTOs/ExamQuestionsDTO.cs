using ExaminationSystem.DTOs.ChoiceDTOs;

namespace ExaminationSystem.DTOs.QuestionDTOs;

public class ExamQuestionsDTO
{
    public int QuestionID { get; set; }
    public string QuestionText { get; set; } = null!;
    public List<QuestionChoicesDTO> Choices { get; set; } = null!;

}
