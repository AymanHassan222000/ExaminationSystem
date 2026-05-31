using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.AuthDTOs;

namespace ExaminationSystem.BLL.Interfaces;

public interface IAuthService
{
    Task<Response<AuthResponseDTO>> LoginAsync(LoginRequestDTO dto);
    Task<Response<bool>> StudentRegisterationAsync(RegisterStudentDTO dto);
    Task<Response<bool>> InstructorRegisterationAsync(RegisterInstructorDTO dto);
    Task<Response<AuthResponseDTO>> RefreshTokenAsync(string token);
    Task<Response<bool>> RevokeTokenAsync(string token);
}
