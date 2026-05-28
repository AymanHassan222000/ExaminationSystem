using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.AuthDTOs;

namespace ExaminationSystem.ViewModels.AuthViewModels;

public class AuthViewModelProfile : Profile
{
    public AuthViewModelProfile()
    {
        CreateMap<LoginRequestViewModel, LoginRequestDTO>();

        CreateMap<Response<AuthResponseDTO>, ResponseViewModel<AuthResponseViewModel>>()
                .ForMember(dest => dest.Data,opt => opt.MapFrom(src => src.Data));

        CreateMap<AuthResponseDTO, AuthResponseViewModel>();

        CreateMap<RegisterStudentViewModel, RegisterStudentDTO>();
    }
}
