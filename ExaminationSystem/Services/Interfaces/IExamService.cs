using ExaminationSystem.DTOs.ExamDTOs;

namespace ExaminationSystem.Services.Interfaces;

public interface IExamService
{
    Task<ResponseDTO<IEnumerable<ExamDetailsDTO>>> GetAllExamsAsync(int instructorID);
    Task<ResponseDTO<ExamDetailsDTO>> GetExamByIDAsync(int examID, int instructorID);
    Task<ResponseDTO<ExamDetailsDTO>> UpdateExamAsync(int examID, int instructorID, UpdateExamDTO dto);
    Task DeleteExamAsync(int id, int instructorID);
}
