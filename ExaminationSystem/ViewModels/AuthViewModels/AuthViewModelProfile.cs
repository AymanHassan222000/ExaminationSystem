using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.AuthDTOs;

namespace ExaminationSystem.ViewModels.AuthViewModels;

public class AuthViewModelProfile : Profile
{
    public AuthViewModelProfile()
    {
        CreateMap<LoginRequestViewModel, LoginRequestDTO>();

        CreateMap<AuthResponseDTO, AuthResponseViewModel>();

        CreateMap<RegisterStudentViewModel, RegisterStudentDTO>();
    }
}
