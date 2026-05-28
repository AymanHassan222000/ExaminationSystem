using ExaminationSystem.DTOs.ChoiceDTOs;

namespace ExaminationSystem.DTOs.QuestionDTOs;


public record AddQuestionDTO(
    string Head,
    int Grade,
    QuestionLevel Level,
    int CourseID,
    IList<AddChoiceDTO> Choices
);

//public sealed class AddQuestionDTO
//{
//    public string Head { get; set; } = null!;
//    public int Grade { get; set; }  
//    public QuestionLevel Level { get; set; }
//    public int CourseID { get; set; }

//    public IList<AddChoiceDTO> Choices { get; set; }
//}
