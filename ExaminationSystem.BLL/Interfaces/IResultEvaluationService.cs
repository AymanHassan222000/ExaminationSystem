using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.ResultEvaluationDTOs;

namespace ExaminationSystem.BLL.Interfaces;

public interface IResultEvaluationService
{
    Task<Response<EvaluateExamResponseDTO>> EvaluateExamAsync(int examID);
    Task<Response<IEnumerable<ExamResultSummaryDTO>>> GetAllStudentResultAsync(int examID);
}
