using ExaminationSystem.API.ViewModels.ChoiceViewModels;
using ExaminationSystem.API.ViewModels.QuestionViewModels;
using ExaminationSystem.BLL.DTOs.QuestionDTOs;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.ChoiceDTOs;
using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.ViewModels.CourseViewModels;

namespace ExaminationSystem.ViewModels.QuestionViewModel;

public class QuestionViewModelProfile : Profile
{
    public QuestionViewModelProfile()
    {
        CreateMap<AddQuestionViewModel, AddQuestionDTO>()
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices));

        CreateMap<GetAllQuestionsDTO, GetAllQuestionsViewModel>();

        CreateMap<GetQuestionByIdResponseDTO, GetQuestionByIdResponseViewModel>()
            .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course))
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices));

        CreateMap<GetCourseInfoDTO, GetCourseInfoViewModel>();
        CreateMap<GetChoicesInfoDTO, GetChoicesInfoViewModel>();

        CreateMap<UpdateQuestionViewModel, UpdateQuestionDTO>();

        CreateMap<UpdateQuestionChoiceViewModel, UpdateQuestionChoiceDTO>();

        CreateMap<AddChoiceToQuestionViewModel, AddChoiceToQuestionDTO>()
            .ForMember(dest => dest.Choices, opt => opt.MapFrom(src => src.Choices));

    }
}
