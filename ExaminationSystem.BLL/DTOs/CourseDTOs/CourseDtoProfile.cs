using ExaminationSystem.BLL.DTOs.CourseDTOs;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DTOs.IntructorDTOs;

namespace ExaminationSystem.DTOs.CourseDTOs;

public class CourseDtoProfile : Profile
{
    public CourseDtoProfile()
    {
        CreateMap<Course, CourseDetailsDTO>()
            .ForMember(dest => dest.Instructor, opt => opt.MapFrom(src => src.Instructor));

        CreateMap<AddCourseDTO, Course>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom( _=> DateTime.UtcNow));

        CreateMap<Course, GetCourseInfoDTO>();

        CreateMap<UpdateCourseDTO, Course>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_=> DateTime.UtcNow));

        CreateMap<AddCoursePrerequisiteDTO, PreRequesit>();
    }
}
