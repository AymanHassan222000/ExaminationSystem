using AutoMapper;
using ExaminationSystem.Models;

namespace ExaminationSystem.DTOs.QuestionDTOs;

public class QuestionDtoProfile : Profile
{
    public QuestionDtoProfile()
    {
        CreateMap<Question, QuestionDetailsDTO>()
            .ForMember(dest => dest.instructorInfo, opt => opt.MapFrom(src => src.Instructor));

        CreateMap<CreateQuestionDTO, Question>();
    }
}
