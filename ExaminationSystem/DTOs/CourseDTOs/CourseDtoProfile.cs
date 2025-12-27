using AutoMapper;
using ExaminationSystem.Models;

namespace ExaminationSystem.DTOs.CourseDTOs;

public class CourseDtoProfile : Profile
{
    public CourseDtoProfile()
    {
        CreateMap<Course, CourseDetailsDTO>()
            .ForMember(dest => dest.instructorInfo , opt => opt.MapFrom(src => src.Instructor));

        CreateMap<CreateCourseDTO, Course>();

        CreateMap<Course, GetCourseInfoDTO>();
    }
}
