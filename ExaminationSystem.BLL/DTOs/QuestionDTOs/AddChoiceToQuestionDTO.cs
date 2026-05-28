using ExaminationSystem.DTOs.ChoiceDTOs;

namespace ExaminationSystem.BLL.DTOs.QuestionDTOs;

public record AddChoiceToQuestionDTO(int QuestionID, IList<AddChoiceDTO> Choices);
