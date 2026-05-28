using ExaminationSystem.DAL.Models;
using ExaminationSystem.DTOs.ChoiceDTOs;
using ExaminationSystem.DTOs.CourseDTOs;

namespace ExaminationSystem.DTOs.QuestionDTOs;

public class QuestionDtoProfile : Profile
{
    public QuestionDtoProfile()
    {

        CreateMap<AddQuestionDTO, Question>()
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => 
                src.Choices.Select(c => new Choice 
                {
                    ChoiceText = c.Text,
                    IsCorrect = c.IsCorrect
                })
            ));

        CreateMap<UpdateQuestionDTO, Question>()
            .ForMember(q => q.Choices, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Question, GetAllQuestionsDTO>();

        CreateMap<Question, GetQuestionByIdResponseDTO>()
            .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course))
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices.Where(c => !c.IsDeleted)));

        CreateMap<Course, GetCourseInfoDTO>()
            .ForMember(dest => dest.Instructor, opt => opt.MapFrom(src => src.Instructor));

        CreateMap<Choice, GetChoicesInfoDTO>();


    }
}
