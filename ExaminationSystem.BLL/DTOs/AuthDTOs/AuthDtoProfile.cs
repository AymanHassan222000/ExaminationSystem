using ExaminationSystem.DAL.Models;
using ExaminationSystem.Helpers.Auth;

namespace ExaminationSystem.DTOs.AuthDTOs;

public class AuthDtoProfile : Profile
{
    public AuthDtoProfile()
    {
        CreateMap<RegisterInstructorDTO, User>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordHasher.Hash(src.Password)))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(_=> UserRoles.Instructor))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_=> false));
        
        CreateMap<RegisterStudentDTO, User>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordHasher.Hash(src.Password)))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(_=> UserRoles.Student));
    }
}
