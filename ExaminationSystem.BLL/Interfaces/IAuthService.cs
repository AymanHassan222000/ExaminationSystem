using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.AuthDTOs;

namespace ExaminationSystem.BLL.Interfaces;

public interface IAuthService
{
    Task<Response<AuthResponseDTO>> LoginAsync(LoginRequestDTO dto);
    Task<Response<AuthResponseDTO>> StudentRegisterationAsync(RegisterStudentDTO dto);
    Task<Response<AuthResponseDTO>> InstructorRegisterationAsync(RegisterInstructorDTO dto);
}
