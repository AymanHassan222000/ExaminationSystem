using ExaminationSystem.DTOs.InstructorDTOs;

namespace ExaminationSystem.Services.Interfaces;

public interface IInstructorService
{
    Task<ResponseDTO<bool>> CreateExamManuallyAsync(CreateExamManualDTO dto, UserContextDTO userContext);
    Task<ResponseDTO<bool>> CreateExamAutoAsync(CreateExamAutoDTO dto, UserContextDTO userContext);
    Task<ResponseDTO<bool>> AssignExamToStudentAsync(AssignExamToStudentDTO dto, UserContextDTO userContext);
}
