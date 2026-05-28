using ExaminationSystem.BLL.DTOs.ExamDTOs;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.DTOs.StudentDTOs;

namespace ExaminationSystem.BLL.Interfaces;

public interface IExamService
{
    Task<Response<bool>> CreateExamManuallyAsync(CreateExamManualDTO dto);
    Task<Response<bool>> CreateExamAutoAsync(CreateExamAutoDTO dto);
    Task<Response<IEnumerable<GetAllExamsDTO>>> GetAllExamsAsync();
    Task<Response<GetExamByIdDTO>> GetExamByIDAsync(int examID);
    Task<Response<bool>> UpdateExamAsync(UpdateExamDTO dto);
    Task<Response<bool>> AddQuestionsToExamAsync(AddingQuestionsToExamDTO dto);
    Task<Response<bool>> RemoveQuestionsFromExamAsync(RemoveQuestionsFromExamDTO dto);
    Task<Response<bool>> DeleteExamAsync(DeleteExamDTO dto);
    Task<Response<GetExamByIdDTO>> TakeExamAsync(int examID);
    Task<Response<bool>> SubmitExamAsync(SubmitExamRequestDTO dto);
    Task<Response<bool>> AssignExamToStudentAsync(AssignExamToStudentDTO dto);
}
