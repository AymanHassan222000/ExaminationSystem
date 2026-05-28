using ExaminationSystem.DTOs.QuestionDTOs;

namespace ExaminationSystem.DTOs.ExamDTOs;

public class GetExamByIdDTO
{
    public string Title { get; set; } = null!;
    public ExamTypes Type { get; set; }
    public int DurationInMinutes { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public IList<ExamQuestionsDTO> Questions { get; set; }
}
