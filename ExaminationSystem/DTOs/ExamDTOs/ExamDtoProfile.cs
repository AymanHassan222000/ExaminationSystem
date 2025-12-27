using AutoMapper;
using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.Models;

namespace ExaminationSystem.DTOs.ExamDTOs;

public class ExamDtoProfile : Profile
{
    public ExamDtoProfile()
    {
        CreateMap<CreateExamDTO, Exam>();

        CreateMap<Exam, ExamDetailsDTO>()
            .ForMember(dest => dest.CourseInfo, opt => opt.MapFrom(src => src.Course));

    }
}
