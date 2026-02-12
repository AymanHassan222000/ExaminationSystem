using ExaminationSystem.DTOs.AuthDTOs;

namespace ExaminationSystem.Services.Interfaces;

public interface IAuthService
{
    Task<ResponseDTO<AuthResponseDTO>> LoginAsync(LoginRequestDTO dto);
    Task<ResponseDTO<AuthResponseDTO>> StudentRegisterationAsync(RegisterStudentDTO dto);
    Task<ResponseDTO<AuthResponseDTO>> InstructorRegisterationAsync(RegisterInstructorDTO dto);
}
