using ExaminationSystem.DTOs.ChoiceDTOs;
using ExaminationSystem.DTOs.CourseDTOs;

namespace ExaminationSystem.DTOs.QuestionDTOs;

public sealed class GetQuestionByIdResponseDTO
{
    public int ID { get; set; }
    public string Head { get; set; } = null!;
    public int Grade { get; set; }
    public QuestionLevel Level { get; set; }

    public GetCourseInfoDTO Course { get; set; }
    public IEnumerable<GetChoicesInfoDTO> Choices { get; set; }
}
