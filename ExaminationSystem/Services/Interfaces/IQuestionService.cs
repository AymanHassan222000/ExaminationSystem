using ExaminationSystem.DTOs.QuestionDTOs;

namespace ExaminationSystem.Services.Interfaces;

public interface IQuestionService
{
    Task<ResponseDTO<QuestionDetailsDTO>> AddQuestionAsync(CreateQuestionDTO dto);
    Task<ResponseDTO<IEnumerable<QuestionDetailsDTO>>> GetAllQuestionsAsync(int instructorID);
    Task<ResponseDTO<QuestionDetailsDTO>> GetQuestionByIDAsync(int questionID, int instructorID);
    Task<ResponseDTO<QuestionDetailsDTO>> UpdateQuestionAsync(int questionID, int instructorID, UpdateQuestionDTO dto);
    Task<ResponseDTO<QuestionDetailsDTO>> DeleteQuestionAsync(int questionID, int instructorID);
}
