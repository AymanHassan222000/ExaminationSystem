using ExaminationSystem.BLL.DTOs.ChoiceDTOs;
using ExaminationSystem.BLL.DTOs.QuestionDTOs;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DTOs;

namespace ExaminationSystem.BLL.Interfaces;

public interface IChoiceService
{
    Task<bool> AddChoicesAsync(IList<Choice> choices);
    Task<Response<bool>> UpdateChoiceAsync(UpdateQuestionChoiceDTO dto);
    Task<Response<bool>> RemoveChoiceAsync(int choiceID);
    Task<Response<IList<GetChoicesInfoForEvaluation>>> GetChoicesInfoForEvaluationAsync(IList<int> choicesIDs);
}
