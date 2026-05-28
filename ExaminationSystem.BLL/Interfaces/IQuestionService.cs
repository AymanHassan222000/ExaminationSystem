using ExaminationSystem.BLL.DTOs.QuestionDTOs;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.QuestionDTOs;

namespace ExaminationSystem.BLL.Interfaces;

public interface IQuestionService
{
    Task<Response<object>> AddQuestionAsync(AddQuestionDTO dto);
    Task<Response<object>> UpdateQuestionAsync(UpdateQuestionDTO dto);
    Task<Response<IEnumerable<GetAllQuestionsDTO>>> GetAllQuestionsAsync();
    Task<Response<GetQuestionByIdResponseDTO>> GetQuestionByIDAsync(int questionID);
    Task<Response<object>> DeleteQuestionAsync(int questionID);
    Task<bool> HasInvalidQuestions(IList<int> questionIDs, int courseID);
    Task<List<int>> GetRandomQuestionIDsAsync(QuestionLevel level, int count, int courseID);
    Task<Dictionary<QuestionLevel, int>> GetQuestionCountsByLevelAsync(int courseId);
    Task<Response<bool>> UpdateQuestionChoice(UpdateQuestionChoiceDTO dto);
    Task<Response<bool>> RemoveChoiceFromQuestionAsync(int choiceID);
    Task<Response<bool>> AddChoicesToQustion(AddChoiceToQuestionDTO dto);

}
