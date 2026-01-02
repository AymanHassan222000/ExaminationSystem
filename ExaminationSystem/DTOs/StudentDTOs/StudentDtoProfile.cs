using AutoMapper;
using ExaminationSystem.Models;

namespace ExaminationSystem.DTOs.StudentDTO;

public class StudentDtoProfile : Profile
{
    public StudentDtoProfile()
    {
        CreateMap<AssignStudentToCourseRequestDTO, StudentCourse>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.StudentID))
                .ForMember(dest => dest.StudetnID, opt => opt.MapFrom(src => src.StudentID));

    }
}
