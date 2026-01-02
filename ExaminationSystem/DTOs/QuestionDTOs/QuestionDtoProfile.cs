using AutoMapper;
using ExaminationSystem.Models;

namespace ExaminationSystem.DTOs.QuestionDTOs;

public class QuestionDtoProfile : Profile
{
    public QuestionDtoProfile()
    {
        CreateMap<Question, QuestionDetailsDTO>()
            .ForMember(dest => dest.instructorInfo, opt => opt.MapFrom(src => src.Instructor))
            .ForMember(dest => dest.QuestionID, opt => opt.MapFrom(src => src.ID));

        CreateMap<CreateQuestionDTO, Question>()
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.InstructorID));

    }
}
