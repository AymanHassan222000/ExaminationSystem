using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.DTOs.ResponseDTOs;
using ExaminationSystem.ViewModels.ResponseViewModels;

namespace ExaminationSystem.ViewModels.QuestionViewModel;

public class QuestionViewModelProfile : Profile
{
    public QuestionViewModelProfile()
    {
        CreateMap<CreateQuestionViewModel, CreateQuestionDTO>();

        CreateMap<QuestionDetailsDTO, QuestionDetailsViewModel>();

        CreateMap<UpdateQuestionViewModel, UpdateQuestionDTO>();

        CreateMap<ResponseDTO<QuestionDetailsDTO>, ResponseViewModel<QuestionDetailsViewModel>>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

        CreateMap<ResponseDTO<IEnumerable<QuestionDetailsDTO>>, ResponseViewModel<IEnumerable<QuestionDetailsViewModel>>>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

    }
}
