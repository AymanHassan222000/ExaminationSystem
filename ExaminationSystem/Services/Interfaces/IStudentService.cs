using ExaminationSystem.DTOs.StudentDTO;
using ExaminationSystem.DTOs.StudentDTOs;

namespace ExaminationSystem.Services.Interfaces;

public interface IStudentService
{
    Task EnrollInCourseAsync(EnrollInCourseRequestDTO dto);
    Task<ResponseDTO<TakeExamResponseDTO>> TakeExamAsync(TakeExamRequestDTO dto,int studentID);
    Task<SubmitExamResponseDTO> SubmitExamAsync(SubmitExamRequestDTO dto);
}
