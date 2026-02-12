using ExaminationSystem.DTOs.ResultEvaluationDTOs;

namespace ExaminationSystem.Services.Interfaces;

public interface IResultEvaluationService
{
    Task<EvaluateExamResponseDTO> EvaluateExamAsync(int examAttemptID);
    Task<IEnumerable<ExamResultSummaryDTO>> GetAllStudentResultAsync(int examID, int instructorID);
    Task<StudentExamResultDTO> GetStudentExamResultAsync(int examAttemptID, int studentID);
}
