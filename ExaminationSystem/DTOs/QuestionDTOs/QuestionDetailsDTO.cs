using ExaminationSystem.DTOs.IntructorDTOs;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.DTOs.QuestionDTOs;

public class QuestionDetailsDTO
{
    public int QuestionID { get; set; }
    public string QuestionText { get; set; } = null!;
    public QuestionLevel Level { get; set; }

    public GetInstructorInfoDTO instructorInfo { get; set; } = null!;
}
