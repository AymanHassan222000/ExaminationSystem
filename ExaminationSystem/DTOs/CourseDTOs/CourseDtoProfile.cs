namespace ExaminationSystem.DTOs.CourseDTOs;

public class CourseDtoProfile : Profile
{
    public CourseDtoProfile()
    {
        CreateMap<Course, CourseDetailsDTO>()
            .ForMember(dest => dest.instructorInfo , opt => opt.MapFrom(src => src.Instructor));

        CreateMap<CreateCourseDTO, Course>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom( _=> DateTime.UtcNow));

        CreateMap<Course, GetCourseInfoDTO>();

        CreateMap<UpdateCourseDTO, Course>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_=> DateTime.UtcNow));

    }
}
